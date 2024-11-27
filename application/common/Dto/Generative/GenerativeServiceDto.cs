using System.Text.Json.Nodes;
using MyApi.Application.Common.Enums;
using MyApi.Application.Common.Interfaces;
// using Newtonsoft.Json.Linq;


namespace Application.Common.Dto.Generative
{
    public class GenerateContentResult : IGenerateContentResult
    {
        public string? ConcatResult { get; set; }
        public string? RawResult { get; set; }
        public JsonArray? JsonParsedRawResult { get; set; }
    }

    public class GenerativeContentOptions : IGenerativeContentOptions
    {
        public List<string>? ImagePathList { get; set; }

        public string? Prompt { get; set; }

        public GenerativeModelEnum? ModelId { get; set; }
    }
}