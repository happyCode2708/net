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
    public class GenerativeConfig: IGenerativeConfig
    {

        public List<object> SafetySettings { get; set; }
        public GenerationConfigModel GenerationConfig { get; set; }
        private string _locationId = "us-central1";
        public string LocationId
        {
            get => _locationId;
            set => _locationId = value;
        }

        private string _projectId = "splendid-sonar-429704-g9";

        public string ProjectId
        {
            get => _projectId;
            set => _projectId = value;
        }

        private string _modelId = GenerativeDict.GetModel[GenerativeModelEnum.Gemini_1_5_Flash_002];

        public string ModelId
        {
            get => _modelId;
            set => _modelId = value;
        }

        public string Token { get; set; }
        private readonly CredentialConfig _credentialConfig;
        public string EndPoint => $"{LocationId}-aiplatform.googleapis.com";
        public string Url => $"https://{EndPoint}/v1/projects/{ProjectId}/locations/{LocationId}/publishers/google/models/{ModelId}:streamGenerateContent";

        public GenerativeConfig(IOptions<CredentialConfig> credentialConfig)
        {
            _credentialConfig = credentialConfig.Value;
            var credential = _credentialConfig.Google;

            SafetySettings = new List<object>
                   {
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" }
                };

            Token = credential;

            //* generationConfig
            GenerationConfig = new GenerationConfigModel
            {
                maxOutputTokens = 8192,
                temperature = 1,
                topP = 0.95,
                topK = 40,
            };

        }

        public void SetModelId(string modelId)
        {
            _modelId = modelId;
        }

        public class GenerationConfigModel
        {
            public double temperature { get; set; }
            public int maxOutputTokens { get; set; }
            public double topP { get; set; }
            public int topK { get; set; }
        }
    }
}

