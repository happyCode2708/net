using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Application.Common.Dto;
using MyApi.Domain.Models;
using static MyApi.Application.Common.Dto.GridDto;
using MyApi.Application.Common.Dto;
using MyApi.Application.Common.Utils;

namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductListResponse
    {
        [JsonIgnore]
        public List<ProductWithImageAndExtraction> ProductList;

        public PaginationInfo Pagination;
    }

    public class ProductWithImage
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ValidationResult { get; set; }
        public ICollection<ProductImageDto>? ProductImages { get; set; } // Navigation property for the join table                                  // public List<ExtractSession> ExtractSessions { get; set; }
        public string? CompareResult { get; set; }
    }


    public class ProductWithImageAndExtraction : ProductExtractionData;


    public class BaseExtractSession
    {
        public ExtractStatus? ExtractionStatus { get; set; }
    }

    public class NutritionInfo : BaseExtractSession
    {
        public NutritionFactData? data { get; set; }
    }

    public class FirstAttributeInfo : BaseExtractSession
    {
        public NutritionFactData? data { get; set; }
    }
    public class ProductExtractionData
    {
        public NutritionInfo? NutritionInfoData { get; set; }
        public FirstAttributeInfo? FirstAttributeInfoData { get; set; }
    }
}