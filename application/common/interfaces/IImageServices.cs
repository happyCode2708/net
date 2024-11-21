using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dto.Image;
using MyApi.Domain.Models;

namespace MyApi.Application.Common.Interfaces
{
    public interface IImageServices
    {
        Task<Image> AddImage(Image image, CancellationToken cancellationToken = default);

        Task ReplaceImage(string originImageId, IFormFile newImageFile, CancellationToken cancellationToken = default);

        Task DeleteImage(string imageId, CancellationToken cancellationToken = default);

        Task<SaveStaticFileReturn> SaveStaticFile(IFormFile file);
    }
}
