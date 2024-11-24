using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Application.Common.Dto.Image;
using Microsoft.Extensions.Options;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using MyApi.Domain.Models;

namespace MyApi.Application.Services
{
    public class ImageServices : IImageServices
    {

        private readonly StorageConfig _storageConfig;

        private readonly IApplicationDbContext _context;

        private readonly IAssetPathService _assetPathService;

        public ImageServices(
            IOptions<StorageConfig> storageConfig,
            IApplicationDbContext context,
            IAssetPathService assetPathService)
        {
            _storageConfig = storageConfig.Value;
            _context = context;
            _assetPathService = assetPathService;
        }

        public async Task<SaveStaticFileReturn> SaveStaticFile(
            IFormFile file,
            int? thumbnailSize = null,
            string? existingImageName = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty");
                }

                // Generate file name
                string imageName;
                string storedFileName;
                string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!string.IsNullOrEmpty(existingImageName))
                {
                    // Use existing filename for thumbnails
                    imageName = existingImageName;
                    storedFileName = $"{existingImageName}{fileExtension}";
                }
                else
                {
                    // Create new filename for original image
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var shortUuid = Guid.NewGuid().ToString("N").Substring(0, 8);
                    imageName = $"{timestamp}_{shortUuid}";
                    storedFileName = $"{timestamp}_{shortUuid}{fileExtension}";
                }

                // Determine subfolder path
                string subFolder;
                if (thumbnailSize.HasValue)
                {
                    subFolder = Path.Combine("images", "thumbnails", thumbnailSize.Value.ToString());
                }
                else
                {
                    subFolder = Path.Combine("images", "originals");
                }

                // var assetPath = Environment.GetEnvironmentVariable("ASSET_PATH");

                // if (string.IsNullOrEmpty(assetPath))
                // {
                //     assetPath = _storageConfig.DefaultAssetPath;
                // }
                var assetPath = _assetPathService.GetAssetPath();

                var fullDirectoryPath = Path.Combine(assetPath, subFolder);
                var filePath = Path.Combine(fullDirectoryPath, storedFileName);

                // Ensure path is normalized
                filePath = Path.GetFullPath(filePath);

                // Validate path
                // if (!filePath.StartsWith(_storageConfig.AssetPath))
                // {
                //     throw new SecurityException("Invalid file path");
                // }

                if (!Directory.Exists(fullDirectoryPath))
                {
                    Directory.CreateDirectory(fullDirectoryPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var normalizedSubFolder = subFolder.Replace(Path.DirectorySeparatorChar, '/');
                var fileUrl = $"{_storageConfig.AssetPathRequest.TrimEnd('/')}/{normalizedSubFolder}/{storedFileName}";

                return new SaveStaticFileReturn
                {
                    OriginFullFileName = file.FileName,
                    StoredFullFileName = storedFileName,
                    ImageName = imageName,
                    ThumbnailSize = thumbnailSize,
                    Extension = fileExtension
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error saving file", ex);
            }
        }

        public async Task<Image> AddImage(Image image, CancellationToken cancellationToken = default)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync(cancellationToken);
            return image;
        }

        public Task DeleteImage(string ImageId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceImage(string originImageId, IFormFile newImageFile, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public string GetImageUrl(Image image, int? thumbnailSize = null)
        {
            string subFolder = thumbnailSize.HasValue
                ? $"images/thumbnails/{thumbnailSize.Value}"
                : "images/originals";

            var normalizedPath = _storageConfig.AssetPathRequest.TrimEnd('/');
            return $"{normalizedPath}/{subFolder}/{image.ImageName}{image.Extension}";
        }

        public string GetImagePath(Image image, int? thumbnailSize = null)
        {
            string subFolder = thumbnailSize.HasValue
                ? Path.Combine("images", "thumbnails", thumbnailSize.Value.ToString())
                : Path.Combine("images", "originals");

            var assetPath = _assetPathService.GetAssetPath();
            var path = Path.Combine(assetPath, subFolder, $"{image.ImageName}{image.Extension}");
            return Path.GetFullPath(path);
        }
    }
}