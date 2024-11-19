using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Enums;
using MyApi.Domain.Models;
using Newtonsoft.Json.Linq;

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

    public class GenerativeContentOptions : IGenerativeContentOptions
    {
        public List<string>? ImagePathList { get; set; }

        public string? Prompt { get; set; }

        public GenerativeModelEnum? ModelId { get; set; }
    }

    public interface IGenerateContentResult
    {
        string? ConcatResult { get; set; }
        string? RawResult { get; set; }
    }

    public class GenerateContentResult : IGenerateContentResult
    {
        public string? ConcatResult { get; set; }
        public string? RawResult { get; set; }
        public JArray? JsonParsedRawResult { get; set; }
    }
}

