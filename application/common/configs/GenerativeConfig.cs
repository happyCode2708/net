using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MyApi.application.common.configs
{
    public class GenerativeConfig
    {

        public List<object> SafetySettings { get; set; }
        public object Config { get; set; }
        public string _locationId = "asia-east3";
        public string LocationId
        {
            get => _locationId;
            set => _locationId = value;
        }

        public string EndPoint => $"https://{LocationId}-aiplatform.googleapis.com";

        public string Token { get; set; }

        private readonly CredentialConfig _credentialConfig;

        public GenerativeConfig(IOptions<CredentialConfig> credentialConfig)
        {
            _credentialConfig = credentialConfig.Value;
            var credential = _credentialConfig.google;

            SafetySettings = new List<object>
                   {
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" }
                };

            Token = credential;

            Config = new
            {
                projectId = "splendid-sonar-429704-g9",
                modelId = "gemini-1.5-flash-001",
                maxOutputTokens = 8192,
                temperature = 0.2,
                topP = 0.95
            };

        }
    }
}