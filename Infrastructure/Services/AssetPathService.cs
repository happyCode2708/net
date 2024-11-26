using MyApi.Application.Common.Interfaces;

namespace MyApi.Infrastructure.Services
{
    public class AssetPathService : IAssetPathService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AssetPathService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public string GetAssetPath()
        {
            var assetPath = Environment.GetEnvironmentVariable("ASSET_PATH");

            if (!string.IsNullOrEmpty(assetPath))
            {
                assetPath = _configuration.GetValue<string>("StorageConfig:DefaultAssetPath");
            }

            return assetPath ?? "assets";
        }
    }
}