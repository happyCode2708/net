using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using MediatR;
using MyApi.application.common.interfaces;
using MyApi.core.controllers;
using MyApi.data;
using MyApi.Models;

namespace MyApi.application.handlers.products.commands.createProduct
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

            private readonly IApplicationDbContext _context;
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
                    ProductId = newProduct.Id
                };


                return ResponseModel<CreateProductResponse>.Success(productResponse);

                // throw new NotImplementedException();
            }
        }
    }
}