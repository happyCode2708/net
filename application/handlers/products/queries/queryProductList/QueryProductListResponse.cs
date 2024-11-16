using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Application.Common.Dto;
using MyApi.Domain.Models;
using static MyApi.Application.Common.Dto.GridDto;
using MyApi.Application.Common.Utils;
using MyApi.Application.Common.Interfaces;
namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductListResponse
    {
        [JsonIgnore]
        public List<ProductGridItemWithExtractionResult> ProductList;

        public PaginationInfo Pagination;
    }

    public class ProductGridItemWithExtractionResult : ProductGridItem
    {
        public ProductExtractionData? ExtractionData { get; set; }
    }

    public class BaseExtractSession
    {
        public ExtractStatus? ExtractionStatus { get; set; }
    }

    public class NutritionInfo : BaseExtractSession
    {
        public NutritionFactData? Data { get; set; }
    }

    public class FirstAttributeInfo : BaseExtractSession
    {
        public NutritionFactData? Data { get; set; }
    }
    public class ProductExtractionData
    {
        public NutritionInfo? NutritionInfoData { get; set; }
        public FirstAttributeInfo? FirstAttributeInfoData { get; set; }
    }
}