using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Domain.Models;
using Application.Common.Utils.ExtractionParser.FirstAttr;
using Application.Common.Utils.ExtractionParser.SecondAttr;
using Application.Common.Dto.Grid;
using Application.Common.Dto.Product;
using Application.Common.Dto.Extraction;

namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductListResponse
    {
        public List<ProductGridItemWithExtractionResult> ProductList { get; set; }

        public PaginationInfo Pagination { get; set; }
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