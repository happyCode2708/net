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
        Task<string> RetrieveImagesInfo(List<Image> images);
        public string EncodeImageToBase64(string imagePath);
    }

    public class GenerativeContentOptions
    {
        public List<string>? ImagePathList { get; set; }

        public string? Prompt { get; set; }

        public GenerativeModelEnum? ModelId { get; set; }
    }

    public class GenerateContentResult
    {
        public string? ConcatResult { get; set; }
        public JArray? JsonParsedRawResult { get; set; }
        public string? RawResult { get; set; }
    }
}

