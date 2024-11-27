using MyApi.Application.Common.Types.Htttp;

namespace Application.Common.Types.Generative
{
    public class GenerationConfigModel
    {
        public double temperature { get; set; }
        public int maxOutputTokens { get; set; }
        public double topP { get; set; }
        public int topK { get; set; }
        public string? responseMimeType { get; set; } = ResponseMimeType.TextPlain;
    }
}