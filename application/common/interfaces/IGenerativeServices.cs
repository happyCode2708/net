using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.application.common.enums;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IGenerativeServices
    {
        Task<string> GenerateContentAsync(GenerativeContentOptions GenerateOptions);
        Task<string> RetrieveImagesInfo(List<Image> images);
        public string EncodeImageToBase64(string imagePath);
    }

    public class GenerativeContentOptions
    {
        public List<string>? ImagePathList { get; set; }

        public string? Prompt { get; set; }

        public GenerativeModelEnum? ModelId { get; set; }


    }
}

