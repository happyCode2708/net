using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Handlers.Products.Commands.CreateProduct;
using MyApi.Core.Controllers;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.CreateProductWithImages
{
    public class CreateProductWithImageCommand : IRequest<ResponseModel<CreateProductWithImagesResponse>>
    {
        public CreateProductWithImagesRequest Request { get; }

        public CreateProductWithImageCommand(CreateProductWithImagesRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<CreateProductWithImageCommand, ResponseModel<CreateProductWithImagesResponse>>
        {

            private readonly IProductServices _productServices;
            private readonly IImageServices _imageServices;

            public Handler(IProductServices productServices, IImageServices imageServices)
            {
                _productServices = productServices;
                _imageServices = imageServices;
            }

            public async Task<ResponseModel<CreateProductWithImagesResponse>> Handle(CreateProductWithImageCommand request, CancellationToken cancellationToken)
            {

                var productRequest = request.Request;

                var newProductWithImages = new Product
                {
                    IxoneID = productRequest?.IxoneID,
                    Upc12 = productRequest?.Upc12,
                    ProductImages = new List<ProductImage>(),
                };

                var requestFiles = request.Request.Files;

                if (requestFiles != null && requestFiles.Count > 0)
                {
                    foreach (var file in requestFiles)
                    {
                        var saveStaticResult = await _imageServices.SaveStaticFile(file);

                        var image = new Image
                        {
                            OriginFileName = saveStaticResult.OriginFileName,
                            StoredFileName = saveStaticResult.StoredFileName,
                            Url = saveStaticResult.FileUrl,
                            Path = saveStaticResult.FilePath,
                        };

                        newProductWithImages.ProductImages.Add(new ProductImage
                        {
                            Product = newProductWithImages,
                            Image = image,
                        });

                    }
                }

                await _productServices.AddProduct(newProductWithImages);

                var newProductWithImagesResponse = new CreateProductWithImagesResponse
                {
                    ProductId = newProductWithImages.Id,
                    IxoneID = newProductWithImages.IxoneID,
                    Upc12 = newProductWithImages.Upc12,
                    ProductImages = newProductWithImages.ProductImages.Select(pi => new CreateProductWithImagesResponseProductImageDto
                    {
                        FileName = pi.Image.OriginFileName,
                        FileUrl = pi.Image.Url,
                    }).ToList(),
                };


                return ResponseModel<CreateProductWithImagesResponse>.Success(newProductWithImagesResponse);

            }
        }

    }
}