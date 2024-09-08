using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IImageServices
    {
        Task<Image> AddImage(Image image, CancellationToken cancellationToken = default);

        Task ReplaceImage(string originImageId, IFormFile newImageFile, CancellationToken cancellationToken = default);

        Task DeleteImage(string imageId, CancellationToken cancellationToken = default);

        Task<SaveStaticFileReturn> SaveStaticFile(IFormFile file);
    }

    public class SaveStaticFileReturn
    {
        public string OriginFileName { get; set; }
        public string StoredFileName { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
    }
}