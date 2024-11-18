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
    public class GenerativeServices : IGenerativeServices
    {
        private readonly IOptions<CredentialConfig> _credentialConfig;

        private readonly IHttpClientFactory _httpClientFactory;

        private string _accessToken;

        private readonly Task _setupGoogleTokenTask;

        private DateTime _tokenExpTime;

        public GenerativeServices(IOptions<CredentialConfig> credentialConfig, IHttpClientFactory httpClientFactory)
        {
            _credentialConfig = credentialConfig;
            _httpClientFactory = httpClientFactory;
            _setupGoogleTokenTask = SetupGoogleTokenAsync();
        }

        private async Task SetupGoogleTokenAsync()
        {
            if (String.IsNullOrEmpty(_accessToken) || DateTime.UtcNow > _tokenExpTime)
            {
                GoogleCredential credential;

                byte[] decodedBytes = System.Convert.FromBase64String(_credentialConfig.Value.google);
                string jsonContent = System.Text.Encoding.UTF8.GetString(decodedBytes);
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
                }

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();


                if (!String.IsNullOrEmpty(accessToken))
                {
                    _accessToken = accessToken;
                    _tokenExpTime = DateTime.UtcNow.AddMinutes(55);
                }
            }
        }

        public async Task<GenerateContentResult> GenerateContentAsync(GenerativeContentOptions generativeOptions)
        {
            await _setupGoogleTokenTask;

            var defaultGenerativeConfig = new GenerativeConfig(_credentialConfig);

            if (generativeOptions.ModelId.HasValue)
            {
                var changeModelConfig = GenerativeModelDict.Map[generativeOptions.ModelId.Value];
                defaultGenerativeConfig.SetModelId(changeModelConfig);
            }

            var imageListInRequest = new List<ImageObject>();
            var partRequest = new List<object>();

            if (generativeOptions.ImagePathList != null)
            {
                imageListInRequest = generativeOptions.ImagePathList.Select(imagePath => new ImageObject
                {
                    inlineData = new InlineData
                    {
                        mimeType = "image/png",
                        data = EncodeImageToBase64(imagePath)
                    }
                }).ToList();
            }

            //* this code is for writeline only -- down
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty JSON output
                                      // ReferenceHandler = ReferenceHandler.Preserve // Handle circular references
            };

            string json = JsonSerializer.Serialize(imageListInRequest, options);
            Console.WriteLine(json); // Lo
            //* this code is for writeline only -- up

            if (!String.IsNullOrWhiteSpace(generativeOptions.Prompt))
            {
                var geminiText = new { text = generativeOptions.Prompt, };
                partRequest.Add(geminiText);

            }

            if (imageListInRequest.Count > 0)
            {
                partRequest.AddRange(imageListInRequest);
            }

            var requestBody = new
            {
                contents = new[] {
                    new {
                        role = "user",
                        parts =  partRequest
                    }
                },
                generationConfig = defaultGenerativeConfig.GenerationConfig,
                safetySettings = defaultGenerativeConfig.SafetySettings,
            };

            Console.WriteLine($"url: {defaultGenerativeConfig.Url}");

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // var response = await httpClient.PostAsJsonAsync(defaultGenerativeConfig.Url, requestBody);

            //! MOCK
            // var response = new{
            //     IsSuccessStatusCode = true,
            //     Content = new StringContent(""),
            // };
            return new GenerateContentResult() {
                RawResult = "mock",
                // JsonParsedRawResult =  new List<object>{new { test = true }},
                ConcatResult = "mock",
            };


            // if (response.IsSuccessStatusCode)
            // {
            //     var result = await response.Content.ReadAsStringAsync();
            //     if (!String.IsNullOrEmpty(result))
            //     {
            //         JArray resultInArray = JArray.Parse(result);

            //         var concatResult = String.Join("", resultInArray.Select(r => r["candidates"]?.First?["content"]?["parts"]?.First?["text"]));

            //         var generateResult = new GenerateContentResult
            //         {
            //             RawResult = result,
            //             JsonParsedRawResult = AppJson.Deserialize<JArray>(result),
            //             ConcatResult = concatResult,
            //         };

            //         return generateResult;
            //     }
            //     else
            //     {

            //         return new GenerateContentResult
            //         {
            //             RawResult = null,
            //             ConcatResult = null,
            //         };
            //     }
            // }
            // else
            // {
            //     var errorContent = await response.Content.ReadAsStringAsync();
            //     throw new HttpRequestException($"Error occurred while calling Google AI API: {response.StatusCode}, Content: {errorContent}");
            // }
        }

        public Task<string> RetrieveImagesInfo(List<Image> images)
        {
            throw new NotImplementedException();
        }

        public string EncodeImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        public async Task<GenerateContentResult> GenerateContentWithApiKeyAsync(GenerativeContentOptions generativeOptions)
        {
            var defaultGenerativeConfig = new GenerativeConfig(_credentialConfig);
            
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
            generationConfig.responseMimeType = GenerativeConfig.ResponseMimeType.TextPlain;

            var requestBody = new
            {
                contents,
                generationConfig,
            };
              
            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = $"{defaultGenerativeConfig.Url}?key={_credentialConfig.Value.googleApiKey}";
            
            var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(result))
                {
                    return new GenerateContentResult
                    {
                        RawResult = result,
                        JsonParsedRawResult = AppJson.Deserialize<JArray>(result),
                        ConcatResult = ExtractTextFromResponse(result)
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

            var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_credentialConfig.Value.googleApiKey}";
            
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
            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);
            return responseJson.GetProperty("file").GetProperty("uri").GetString();
        }

        private string ExtractTextFromResponse(string response)
        {
            // Implement response parsing based on the actual response format
            // This is a placeholder - adjust according to actual response structure
            var responseObj = JsonSerializer.Deserialize<JsonElement>(response);
            return responseObj.GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
        }
    }

    public class InlineData
    {
        public string mimeType { get; set; }
        public string data { get; set; }
    }

    public class ImageObject
    {
        public InlineData inlineData { get; set; }
    }
}

