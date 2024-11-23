using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.Win32.SafeHandles;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using MyApi.Domain.Models;
using Google.Apis.Auth.OAuth2;
using System.Text;
using MyApi.Application.Common.Dict;
using Microsoft.OpenApi.Any;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using MyApi.Application.Common.Utils;
using Application.Common.Dto.Gemini;

namespace MyApi.Infrastructure.Services
{
    public class GeminiServices : IGeminiServices
    {
        private readonly IOptions<CredentialConfig> _credentialConfig;

        private readonly IHttpClientFactory _httpClientFactory;
        public GeminiServices(IOptions<CredentialConfig> credentialConfig, IHttpClientFactory httpClientFactory)
        {
            _credentialConfig = credentialConfig;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GeminiGenerateContentResult> GenerateContentWithApiKeyAsync(GeminiGenerativeContentOptions generativeOptions)
        {
            var defaultGenerativeConfig = new GeminiConfig(_credentialConfig);

            var requireModelId = generativeOptions.ModelId;

            if (requireModelId.HasValue)
            {
                var changeModelConfig = GenerativeDict.GetModel[requireModelId.Value];
                defaultGenerativeConfig.SetModelId(changeModelConfig);
            }

            var contents = new List<object>();

            // Add image parts if present
            if (generativeOptions.ImagePathList?.Any() == true)
            {
                foreach (var imagePath in generativeOptions.ImagePathList)
                {
                    contents.Add(new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new
                            {
                                fileData = new
                                {
                                    fileUri = await UploadImageAsync(imagePath),
                                    mimeType = "image/jpeg" // Adjust mime type based on your needs
                                }
                            }
                        }
                    });
                }
            }

            // Add text prompt if present
            if (!string.IsNullOrWhiteSpace(generativeOptions.Prompt))
            {
                contents.Add(new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = generativeOptions.Prompt }
                    }
                });
            }

            var generationConfig = defaultGenerativeConfig.GenerationConfig;

            var requestBody = new
            {
                contents,
                generationConfig,
            };


            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = defaultGenerativeConfig.Url;

            var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(result))
                {
                    JObject resultObject = JObject.Parse(result);
                    var candidates = resultObject?["candidates"];

                    var concatResult = String.Join("", candidates.Select(r => r["content"]?["parts"]?.First?["text"]));

                    return new GeminiGenerateContentResult
                    {
                        RawResult = result,
                        JsonParsedRawResult = resultObject,
                        ConcatResult = concatResult
                    };
                }

                return new GeminiGenerateContentResult();
            }
            else
            {
                throw new HttpRequestException($"Error calling Gemini API: {response.StatusCode}");
            }

        }

        private async Task<string> UploadImageAsync(string imagePath)
        {
            {
                var httpClient = _httpClientFactory.CreateClient();

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_credentialConfig.Value.GoogleApiKey}";

                var fileName = Path.GetFileName(imagePath);
                var metadata = new { file = new { display_name = fileName } };
                var metadataJson = JsonSerializer.Serialize(metadata);

                using var multipartContent = new MultipartFormDataContent();

                // Add metadata part
                var metadataContent = new StringContent(metadataJson, Encoding.UTF8, "application/json");
                metadataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"metadata\""
                };
                multipartContent.Add(metadataContent);

                // Add file part
                var fileBytes = await File.ReadAllBytesAsync(imagePath);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"file\"",
                    FileName = $"\"{fileName}\""
                };
                multipartContent.Add(fileContent);

                using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl) { Content = multipartContent };

                // Send request
                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to upload image: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);

                return responseJson.GetProperty("file").GetProperty("uri").GetString();
            }




            // private string ExtractTextFromResponse(string response)
            // {
            //     // Implement response parsing based on the actual response format
            //     // This is a placeholder - adjust according to actual response structure
            //     var responseObj = JsonSerializer.Deserialize<JsonElement>(response);
            //     return responseObj.GetProperty("candidates")[0]
            //         .GetProperty("content")
            //         .GetProperty("parts")[0]
            //         .GetProperty("text")
            //         .GetString();
            // }
        }

        // public class InlineData
        // {
        //     public string mimeType { get; set; }
        //     public string data { get; set; }
        // }

        // public class ImageObject
        // {
        //     public InlineData inlineData { get; set; }
        // }
    }
}

