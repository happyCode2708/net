using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Domain.Models;
using MyApi.Core.Controllers;


namespace MyApi.Application.Handlers.Products.Commands.CreateProduct
{

    public class CreateProductCommand : IRequest<ResponseModel<CreateProductResponse>>
    {
        public CreateProductRequest Request { get; }

        public CreateProductCommand(CreateProductRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<CreateProductCommand, ResponseModel<CreateProductResponse>>
        {
            private readonly IProductServices _productServices;

            public Handler(IProductServices productServices)
            {
                _productServices = productServices;
            }

            public async Task<ResponseModel<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var productRequest = request.Request;

                var newProduct = new Product
                {
                    IxoneID = productRequest.IxoneID,
                    Upc12 = productRequest.Upc12,
                };

                await _productServices.AddProduct(newProduct);


                var productResponse = new CreateProductResponse
                {
                    ProductId = newProduct.Id,
                    IxoneID = newProduct.IxoneID,
                    Upc12 = newProduct.Upc12,
                };


                return ResponseModel<CreateProductResponse>.Success(productResponse, "create product successfully");
            }
        }
    }
}