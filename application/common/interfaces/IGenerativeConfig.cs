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

    public static class ResponseMimeType
    {
        public const string TextPlain = "text/plain";
    }

    public class GenerationConfigModel
    {
        public double temperature { get; set; }
        public int maxOutputTokens { get; set; }
        public double topP { get; set; }
        public int topK { get; set; }

        // [JsonConverter(typeof(JsonStringEnumConverter))]
        public string? responseMimeType { get; set; }
    }

}