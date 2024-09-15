using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.Win32.SafeHandles;
using MyApi.application.common.configs;
using MyApi.application.common.interfaces;
using MyApi.Models;

namespace MyApi.application.infrastructure.services
{
    public class GenerativeServices : IGenerativeServices
    {
        private readonly CredentialConfig _credentialConfig;

        public GenerativeServices(IOptions<CredentialConfig> credentialConfig)
        {
            _credentialConfig = credentialConfig.Value;
        }

        public Task<string> RetrieveImagesInfo(List<Image> images)
        {
            throw new NotImplementedException();
        }
    }
}