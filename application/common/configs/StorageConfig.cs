namespace MyApi.Application.Common.Configs
{
    public class StorageConfig
    {
        public long MaxFileSize { get; set; }
        public string DefaultAssetPath { get; set; }
        public string AssetPathRequest { get; set; }
    }
}