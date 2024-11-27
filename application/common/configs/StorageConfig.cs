namespace MyApi.Application.Common.Configs
{
    public class StorageConfig
    {
        public long MaxFileSize { get; set; }
        public required string DefaultAssetPath { get; set; }
        public required string AssetPathRequest { get; set; }
    }
}