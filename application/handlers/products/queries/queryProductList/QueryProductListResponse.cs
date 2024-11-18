using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Application.Common.Dto;
using MyApi.Domain.Models;
using static MyApi.Application.Common.Dto.GridDto;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Common.Utils.ParseExtractedResult.FirstAttributeParserUtils;
using MyApi.Application.Common.Utils.ParseExtractedResult.SecondAttributeParserUtils;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

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

    public class ExtendExtractSession
    {
        public ExtractStatus? ExtractionStatus { get; set; }
    }

    public class NutritionExtractResult : ExtendExtractSession
    {
        public NutritionFactData? Data { get; set; }
    }

    public class FirstAttributeExtractResult : ExtendExtractSession
    {
        public FirstProductAttributeInfo? Data { get; set; }
    }


    public class SecondAttributeExtractResult : ExtendExtractSession
    {
        public SecondAttributeProductInfo? Data { get; set; }
    }

    public class ProductExtractionData
    {
        public NutritionExtractResult? NutritionInfo { get; set; }
        public FirstAttributeExtractResult? FirstAttributeInfo { get; set; }
        public SecondAttributeExtractResult? SecondAttributeInfo { get; set; }
    }
}