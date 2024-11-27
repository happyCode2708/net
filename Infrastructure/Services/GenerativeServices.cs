using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using Google.Apis.Auth.OAuth2;
using System.Text;
using MyApi.Application.Common.Dict;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Common.Dto.Generative;
using MyApi.Application.Common.Utils.Base;

namespace MyApi.Infrastructure.Services
{
    public class GenerativeServices : IGenerativeServices
    {
        private readonly IOptions<CredentialConfig> _credentialConfig;

        private readonly IHttpClientFactory _httpClientFactory;

        private string _accessToken = string.Empty;

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

                byte[] decodedBytes = System.Convert.FromBase64String(_credentialConfig.Value.Google);
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

            var requireModelId = generativeOptions.ModelId;

            if (requireModelId.HasValue)
            {
                var changeModelConfig = GenerativeDict.GetModel[requireModelId.Value];
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
                        data = FileUtils.EncodeImageToBase64(imagePath)
                    }
                }).ToList();
            }

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

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsJsonAsync(defaultGenerativeConfig.Url, requestBody);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(result))
                {
                    var resultInArray = JsonSerializer.Deserialize<JsonArray>(result);

                    var concatResult = String.Join("", resultInArray.Select(r =>
                        r.AsObject()["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.GetValue<string>() ?? ""));

                    var generateResult = new GenerateContentResult
                    {
                        RawResult = result,
                        JsonParsedRawResult = JsonSerializer.Deserialize<JsonArray>(result),
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
