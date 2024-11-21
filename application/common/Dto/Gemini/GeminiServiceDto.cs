using MyApi.Application.Common.Interfaces;
using MyApi.Application.Common.Enums;
using Newtonsoft.Json.Linq;

namespace Application.Common.Dto.Gemini
{
    public class GeminiGenerativeContentOptions : IGenerativeContentOptions
    {
        public List<string>? ImagePathList { get; set; }
        public string? Prompt { get; set; }
        public GenerativeModelEnum? ModelId { get; set; }
    }
    public class GeminiGenerateContentResult : IGenerateContentResult
    {
        public string? ConcatResult { get; set; }
        public string? RawResult { get; set; }
        public JObject? JsonParsedRawResult { get; set; }
    }
}