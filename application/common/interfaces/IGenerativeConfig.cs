using System.Text.Json.Serialization;

namespace Application.Common.Interfaces
{

    public interface IGenerativeConfig
    {
        void SetModelId(string modelId);
    }

    public interface IGeminiConfig : IGenerativeConfig
    {
        string? GoogleApiKey { get; set; }
        string? UploadImageUrl { get; }
        void SetGoogleApiKey(string googleApiKey);
    }
}