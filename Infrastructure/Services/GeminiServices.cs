using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using System.Text;
using MyApi.Application.Common.Dict;
using System.Text.Json;

using Application.Common.Dto.Gemini;
using MyApi.Application.Common.Utils.Base;
using System.Text.Json.Nodes;

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

            var result = new GeminiGenerateContentResult
            {
                RawResult = null,
                JsonParsedRawResult = null,
                ConcatResult = null,
            };

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(responseString))
                {
                    var resultObject = JsonSerializer.Deserialize<JsonObject>(responseString);
                    var candidates = resultObject?["candidates"]?.AsArray();

                    var concatResult = String.Join("", candidates?.Select(r => r?["content"]?["parts"]?.AsArray()?.FirstOrDefault()?["text"]?.GetValue<string>() ?? "").Where(c => c != null)!);

                    result = new GeminiGenerateContentResult
                    {
                        RawResult = responseString,
                        JsonParsedRawResult = resultObject,
                        ConcatResult = concatResult
                    };

                    return result;
                }

                return result;
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
                var responseJson = AppJson.Deserialize<JsonElement>(responseContent);

                return responseJson.GetProperty("file").GetProperty("uri").GetString() ?? string.Empty;
            }
        }
    }

}

