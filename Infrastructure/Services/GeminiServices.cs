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

namespace MyApi.Application.Services
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
            
            if (generativeOptions.ModelId.HasValue)
            {
                var changeModelConfig = GenerativeModelDict.Map[generativeOptions.ModelId.Value];
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
            var apiUrl = $"{defaultGenerativeConfig.Url}?key={_credentialConfig.Value.GoogleApiKey}";
            
            var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(result))
                {
                    return new GeminiGenerateContentResult
                    {
                        RawResult = result,
                        JsonParsedRawResult = AppJson.Deserialize<JArray>(result),
                        // ConcatResult = ExtractTextFromResponse(result)
                    };
                }
            }

            throw new HttpRequestException($"Error calling Gemini API: {response.StatusCode}");
        }

        private async Task<string> UploadImageAsync(string imagePath)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var fileBytes = await File.ReadAllBytesAsync(imagePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Adjust based on file type

            var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_credentialConfig.Value.GoogleApiKey}";
            
            using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
            request.Headers.Add("X-Goog-Upload-Command", "start, upload, finalize");
            request.Headers.Add("X-Goog-Upload-Header-Content-Length", fileBytes.Length.ToString());
            request.Headers.Add("X-Goog-Upload-Header-Content-Type", "image/jpeg");
            
            var metadata = new { file = new { display_name = Path.GetFileName(imagePath) } };
            request.Content = new StringContent(JsonSerializer.Serialize(metadata), Encoding.UTF8, "application/json");
            
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to upload image: {response.StatusCode}");

            var responseContent = await response.Content.ReadAsStringAsync();
            // Parse the response to get the file URI
            // var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var responseJson = AppJson.Deserialize<JArray>(responseContent);

            var imgUri = responseJson?[0]?["file"]?["uri"]?.ToString();

            return imgUri ?? "";
            // return responseJson.GetProperty("file").GetProperty("uri").GetString();
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

