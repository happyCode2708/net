using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;
using System.Text.Json.Serialization;
using Application.Common.Interfaces;

namespace MyApi.Application.Common.Configs
{
    public class GeminiConfig : IGeminiConfig
    {

        public List<object> SafetySettings { get; set; }
        public GenerationConfigModel GenerationConfig { get; set; }
        private string _locationId = "us-central1";
        public string LocationId
        {
            get => _locationId;
            set => _locationId = value;
        }


        private string _modelId = GenerativeModelDict.Map[GenerativeModelEnum.Gemini_1_5_Flash_002];

        public string ModelId
        {
            get => _modelId;
            set => _modelId = value;
        }
        private readonly CredentialConfig _credentialConfig;

        private string? _googleApiKey;

        public string? GoogleApiKey
        {
            get => _googleApiKey;
            set => _googleApiKey = value;
        }

        public string EndPoint => $"generativelanguage.googleapis.com";
        public string Url => $"https://{EndPoint}/v1beta/models/{ModelId}:generateContent?key={GoogleApiKey}";
        public string UploadImageUrl => $"https://vision.googleapis.com/v1/images:annotate?key={GoogleApiKey}";

        public GeminiConfig(IOptions<CredentialConfig> credentialConfig)
        {
            _credentialConfig = credentialConfig.Value;
            SetGoogleApiKey(_credentialConfig.GoogleApiKey);

            SafetySettings = new List<object>
                   {
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" }
                };
            //* generationConfig
            GenerationConfig = new GenerationConfigModel
            {
                maxOutputTokens = 8192,
                temperature = 1,
                topP = 0.95,
                topK = 40,
                responseMimeType = "text/plain"
            };

        }

        public void SetGoogleApiKey(string googleApiKey)
        {
            _googleApiKey = googleApiKey;
        }

        public void SetModelId(string modelId)
        {
            _modelId = modelId;
        }
    }
}

