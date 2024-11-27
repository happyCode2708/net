using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MyApi.Domain.Models;
using Application.Common.Dto.Product;
using Application.Common.Dto.Grid;
using Application.Common.Dto.Extraction;
using MyApi.Application.Common.Utils.Base;

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

                var pageNumber = request.PageNumber;
                var pageSize = request.PageSize;

                var getProductGridProps = new GetProductGridProps
                {
                    Filter = new GetProductGridFiler
                    {
                        SearchText = request.SearchText,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    },
                    Options = new GetProductGridOptions
                    {
                        IncludeImages = true
                    },
                };

                var searchProductGridResult = await _productServices.GetProductGrid(getProductGridProps);

                var productIds = searchProductGridResult.ProductGridData.Select(p => p.Id).ToList();

                // Get latest extraction results for products grouped by sourceType
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
                    var firstAttributeInfo = latestExtractionsOfProductItems.Where(e => e.SourceType == ExtractSourceType.FirstAttribute && !String.IsNullOrEmpty(e.ExtractedData)).FirstOrDefault();
                    var secondAttributeInfo = latestExtractionsOfProductItems.Where(e => e.SourceType == ExtractSourceType.SecondAttribute && !String.IsNullOrEmpty(e.ExtractedData)).FirstOrDefault();

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
                            Data = !String.IsNullOrEmpty(secondAttributeInfo?.ExtractedData) ? AppJson.Deserialize<SecondAttributeProductInfo>(secondAttributeInfo.ExtractedData) : null,
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
                        Count = searchProductGridResult?.TotalCount ?? 0,
                        PageNumber = request.PageNumber.Value,
                        PageSize = request.PageSize.Value
                    };
                };


                return ResponseModel<QueryProductListResponse>.Success(response, "Successfully queried product list with extraction data");
            }
        }
    }
}