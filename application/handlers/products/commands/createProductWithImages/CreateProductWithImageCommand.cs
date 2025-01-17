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

                var newProduct = new Product
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
                            ImageName = saveStaticResult.ImageName,
                            Extension = saveStaticResult.Extension,
                            OriginalFileName = saveStaticResult.OriginFullFileName,
                            FileSize = file.Length,
                            MimeType = file.ContentType,
                            IsRaw = true,
                        };

                        image = await _imageServices.AddImage(image, cancellationToken);


                        newProduct.ProductImages.Add(new ProductImage
                        {
                            Product = newProduct,
                            Image = image,
                        });

                    }
                }

                await _productServices.AddProduct(newProduct);

                var newProductResponse = new CreateProductWithImagesResponse
                {
                    ProductId = newProduct.Id,
                    IxoneID = newProduct.IxoneID,
                    Upc12 = newProduct.Upc12,
                    ProductImages = newProduct.ProductImages.Select(pi => new CreateProductWithImagesResponseProductImageDto
                    {
                        OriginalFileName = pi.Image.OriginalFileName,
                        ImageUrl = _imageServices.GetImageUrl(pi.Image),
                    }).ToList(),
                };

                return ResponseModel<CreateProductWithImagesResponse>.Success(newProductResponse, "successfully created product");

            }
        }

    }
}