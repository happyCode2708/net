using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Application.Common.Dto;
using MyApi.Domain.Models;

namespace MyApi.Application.Common.Interfaces
{
    public interface IProductServices
    {
        Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default);

        Task<GetProductGridItemReturn> GetProductGrid(GetProductGridProps props);

        Task UpdateProduct(Product product, CancellationToken cancellationToken = default);

        Task DeleteProduct(int productId, CancellationToken cancellationToken = default);

        Task DeleteImageFromProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);
        Task AddImageToProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);

    }

    public class BaseProductGridItem
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class ProductGridItem : BaseProductGridItem
    {
        public List<ProductImageDto>? ProductImages { get; set; }
    }

    public class GetProductGridItemReturn
    {
        // [JsonIgnore]
        public List<ProductGridItem> ProductGridData;
        public int TotalCount;

    }

    public class GetProductGridProps
    {
        public GetProductGridFiler Filter;

        public GetProductGridOptions? Options;
    }

    public class GetProductGridFiler
    {
        public string? SearchText;

        public int? PageNumber;

        public int? PageSize;
    }

    public class GetProductGridOptions
    {
        public Boolean? IncludeImages;
    }
}