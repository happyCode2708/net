using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using MyApi.Application.Common.Interfaces;
// using MyApi.Application.Handlers.Products.Queries.GetProductList;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
// using MyApi.application.handlers.products.queries.queryProductList;

using MyApi.Core.Controllers;
using static MyApi.Application.Common.Dto.GridDto;

namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductList : IRequest<ResponseModel<QueryProductListResponse>>
    {
        public QueryProductListRequest Request { get; }
        public QueryProductList(QueryProductListRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<QueryProductList, ResponseModel<QueryProductListResponse>>
        {
            private readonly IProductServices _productServices;

            public Handler(IProductServices productServices)
            {
                _productServices = productServices;
            }

            public async Task<ResponseModel<QueryProductListResponse>> Handle(QueryProductList queryProductList, CancellationToken cancellationToken)
            {

                var request = queryProductList.Request;


                var getProductListProps = new GetProductListProps
                {
                    Filter = new GetProductListFiler
                    {
                        SearchText = request.SearchText,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    },
                    Options = new GetProductListOptions
                    {
                        IncludeImages = true
                    },
                };

                var searchProductListResult = await _productServices.GetProductList(getProductListProps);

                var productList = searchProductListResult.ProductList;


                var response = new QueryProductListResponse
                {
                    ProductList = searchProductListResult.ProductList,
                };

                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    response.Pagination = new PaginationInfo
                    {
                        Count = searchProductListResult.Count,
                        PageNumber = request.PageNumber.Value,
                        PageSize = request.PageSize.Value
                    };
                };

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(searchProductListResult.ProductList));


                return ResponseModel<QueryProductListResponse>.Success(response, "Success fully query product list");

            }
        }
    }
}