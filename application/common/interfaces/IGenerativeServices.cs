using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IGenerativeServices
    {
        Task<string> GenerateContentAsync(GenerativeContentOptions GenerateOptions);
        Task<string> RetrieveImagesInfo(List<Image> images);

    }

    public class GenerativeContentOptions
    {
        public List<string>? Base64Images { get; set; }

        public string? Prompt { get; set; }

        public string? ModelId { get; set; }

    }
}

