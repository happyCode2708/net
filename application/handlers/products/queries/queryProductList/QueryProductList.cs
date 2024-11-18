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
using AutoMapper;
using MyApi.Domain.Models;
using MyApi.Application.Common.Utils;
using MyApi.Application.Common.Utils.ParseExtractedResult.FirstAttributeParserUtils;
using MyApi.Application.Common.Utils.ParseExtractedResult.SecondAttributeParserUtils;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

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
            private readonly IMapper _mapper;

            public Handler(IProductServices productServices, IApplicationDbContext context, IMapper mapper)
            {
                _productServices = productServices;
                _context = context;
                _mapper = mapper;
            }

            public async Task<ResponseModel<QueryProductListResponse>> Handle(QueryProductList queryProductList, CancellationToken cancellationToken)
            {
                var request = queryProductList.Request;

                var getProductGridProps = new GetProductGridProps
                {
                    Filter = new GetProductGridFiler
                    {
                        SearchText = request.SearchText,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    },
                    Options = new GetProductGridOptions
                    {
                        IncludeImages = true
                    },
                };

                var searchProductGridResult = await _productServices.GetProductGrid(getProductGridProps);

                // Get latest extraction results for products grouped by sourceType
                var productIds = searchProductGridResult.ProductGridData.Select(p => p.Id).ToList();

                var latestExtractionsOfProductList = await _context.ExtractSessions
                    .Where(e => productIds.Contains(e.ProductId))
                    .GroupBy(g => new { g.ProductId, g.SourceType })
                    .Select(g => g.OrderByDescending(e => e.CompletedAt).First())
                    .ToListAsync(cancellationToken);

                var productsWithExtractions = _mapper.Map<List<ProductGridItemWithExtractionResult>>(searchProductGridResult.ProductGridData);

                productsWithExtractions.ForEach(p =>
                {
                    var latestExtractionsOfProductItems = latestExtractionsOfProductList.Where(e => e.ProductId == p.Id).ToList();

                    var nutritionInfo = latestExtractionsOfProductItems.Where(e => e.SourceType == ExtractSourceType.NutritionFact && !String.IsNullOrEmpty(e.ExtractedData)).FirstOrDefault();
                    var firstAttributeInfo = latestExtractionsOfProductItems.Where(e => e.SourceType == ExtractSourceType.ProductFirstAttribute && !String.IsNullOrEmpty(e.ExtractedData)).FirstOrDefault();
                    var SecondAttributeInfo = latestExtractionsOfProductItems.Where(e => e.SourceType == ExtractSourceType.ProductSecondAttribute && !String.IsNullOrEmpty(e.ExtractedData)).FirstOrDefault();

                    p.ExtractionData = new ProductExtractionData
                    {
                        NutritionInfo = new NutritionExtractResult
                        {
                            Data = !String.IsNullOrEmpty(nutritionInfo?.ExtractedData) ? AppJson.Deserialize<NutritionFactData>(nutritionInfo.ExtractedData) : null,
                            ExtractionStatus = nutritionInfo?.Status,
                        },
                        FirstAttributeInfo = new FirstAttributeExtractResult
                        {
                            Data = !String.IsNullOrEmpty(firstAttributeInfo?.ExtractedData) ? AppJson.Deserialize<FirstProductAttributeInfo>(firstAttributeInfo.ExtractedData) : null,
                            ExtractionStatus = firstAttributeInfo?.Status,
                        },
                        SecondAttributeInfo = new SecondAttributeExtractResult
                        {
                            Data = !String.IsNullOrEmpty(SecondAttributeInfo?.ExtractedData) ? AppJson.Deserialize<SecondAttributeProductInfo>(SecondAttributeInfo.ExtractedData) : null,
                            ExtractionStatus = firstAttributeInfo?.Status,
                        },                    
                    };
                });


                var response = new QueryProductListResponse
                {
                    ProductList = productsWithExtractions
                };

                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    response.Pagination = new PaginationInfo
                    {
                        Count = searchProductGridResult.TotalCount,
                        PageNumber = request.PageNumber.Value,
                        PageSize = request.PageSize.Value
                    };
                };


                return ResponseModel<QueryProductListResponse>.Success(response, "Successfully queried product list with extraction data");
            }
        }
    }
}