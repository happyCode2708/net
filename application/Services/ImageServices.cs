using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using MyApi.Domain.Models;

namespace MyApi.Application.Services
{
    public class ImageServices : IImageServices
    {

        private readonly StorageConfig _storageConfig;

        public ImageServices(IOptions<StorageConfig> storageConfig)
        {
            _storageConfig = storageConfig.Value;
        }

        public async Task<SaveStaticFileReturn> SaveStaticFile(IFormFile file)
        {

            var originFileName = file.FileName;
            var originFileNameWithoutExtension = Path.GetFileNameWithoutExtension(originFileName);
            var fileExt = Path.GetExtension(originFileName);

            var uuid = Guid.NewGuid().ToString();

            var newStoredFileName = $"{uuid}__{originFileName}";


            var filePath = Path.Combine(_storageConfig.AssetPath, newStoredFileName);
            Console.WriteLine($"filePath = {filePath}");
            var fileDir = Path.GetDirectoryName(filePath);
            Console.WriteLine($"fileDir = {fileDir}");

            Directory.CreateDirectory(fileDir);

            using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            stream.Close();

            var result = new SaveStaticFileReturn
            {
                OriginFileName = originFileName,
                StoredFileName = newStoredFileName,
                FilePath = filePath,
                FileUrl = Path.Combine(_storageConfig.AssetPathRequest, newStoredFileName)
            };

            return result;
        }

        public Task<Image> AddImage(Image image, CancellationToken cancellationToken = default)
        {

            throw new NotImplementedException();
        }

        public Task DeleteImage(string ImageId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceImage(string originImageId, IFormFile newImageFile, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}