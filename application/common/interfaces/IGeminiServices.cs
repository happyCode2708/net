using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MyApi.Application.Common.Enums;
using MyApi.Domain.Models;
using Newtonsoft.Json.Linq;

namespace MyApi.Application.Common.Interfaces
{
    public interface IGeminiServices
    {
        Task<GeminiGenerateContentResult> GenerateContentWithApiKeyAsync(GeminiGenerativeContentOptions GenerateOptions);
    }

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

