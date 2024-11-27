using System.Text.Json.Nodes;
using Application.Common.Dto.Generative;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Common.Interfaces
{
    public interface IGenerativeServices
    {
        Task<GenerateContentResult> GenerateContentAsync(GenerativeContentOptions GenerateOptions);
    }

    public interface IGenerativeContentOptions
    {
        List<string>? ImagePathList { get; set; }
        string? Prompt { get; set; }
        GenerativeModelEnum? ModelId { get; set; }
    }

    public interface IGenerateContentResult
    {
        string? ConcatResult { get; set; }
        string? RawResult { get; set; }
        JsonNode? JsonParsedRawResult { get; set; }
    }
}

