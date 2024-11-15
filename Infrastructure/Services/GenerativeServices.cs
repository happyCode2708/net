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

            var response = await httpClient.PostAsJsonAsync(defaultGenerativeConfig.Url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(result))
                {
                    JArray resultInArray = JArray.Parse(result);

                    var concatResult = String.Join("", resultInArray.Select(r => r["candidates"]?.First?["content"]?["parts"]?.First?["text"]));

                    var generateResult = new GenerateContentResult
                    {
                        RawResult = result,
                        ConcatResult = concatResult,
                    };

                    return generateResult;
                }
                else
                {

                    return new GenerateContentResult
                    {
                        RawResult = null,
                        ConcatResult = null,
                    };
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error occurred while calling Google AI API: {response.StatusCode}, Content: {errorContent}");
            }
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

