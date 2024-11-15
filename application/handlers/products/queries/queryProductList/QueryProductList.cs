using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
using MyApi.Core.Controllers;
using static MyApi.Application.Common.Dto.GridDto;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Dto;

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
            private readonly IApplicationDbContext _context;

            public Handler(IProductServices productServices, IApplicationDbContext context)
            {
                _productServices = productServices;
                _context = context;
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

                // Get latest extraction results for products grouped by sourceType
                var productIds = searchProductListResult.ProductList.Select(p => p.Id).ToList();
                var latestExtractionsOfAllProducts= await _context.ExtractSessions
                    .Where(e => productIds.Contains(e.ProductId))
                    .GroupBy(g => new { g.ProductId, g.SourceType })
                    .Select(g => g.OrderByDescending(e => e.CreatedAt).First())
                    .ToListAsync(cancellationToken);

                // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(latestExtractionsOfAllProducts));
                

                // Combine product and extraction data
                var productsWithExtractions = searchProductListResult.ProductList.Select(product => {
                    // var extractions = latestExtractionsOfAllProducts.Where(e => e.ProductId == product.Id);
                    
                    var productExtractionData = new ProductExtractionData();
                    
                    // foreach(var extraction in latestExtractionsOfAllProducts) {
                    //     if(extraction.ProductId == product.Id) {
                    //     }
                    // }

                    // var mainExtraction = productExtractionData.FirstOrDefault(); // Get first extraction for main status
                    // if (mainExtraction != null)
                    // {
                    //     product.ExtractionData = mainExtraction.ExtractedData;
                    //     product.ExtractionStatus = mainExtraction.Status;
                    // }
                    return product;
                }).ToList();

                var response = new QueryProductListResponse
                {
                    ProductList = productsWithExtractions,
                };

                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    response.Pagination = new PaginationInfo
                    {
                        Count = searchProductListResult.TotalCount,
                        PageNumber = request.PageNumber.Value,
                        PageSize = request.PageSize.Value
                    };
                };

                // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(productsWithExtractions));

                return ResponseModel<QueryProductListResponse>.Success(response, "Successfully queried product list with extraction data");
            }
        }
    }
}