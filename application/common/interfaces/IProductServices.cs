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

        Task<GetProductListReturn> GetProductList(GetProductListProps props);

        Task UpdateProduct(Product product, CancellationToken cancellationToken = default);

        Task DeleteProduct(int productId, CancellationToken cancellationToken = default);

        Task DeleteImageFromProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);
        Task AddImageToProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);

    }

    public class GetProductListReturn
    {
        [JsonIgnore]
        public List<ProductWithImage> ProductList;
        public int Count;

    }

    public class GetProductListProps
    {
        public GetProductListFiler Filter;

        public GetProductListOptions? Options;
    }

    public class GetProductListFiler
    {
        public string? SearchText;

        public int? PageNumber;

        public int? PageSize;
    }

    public class GetProductListOptions
    {
        public Boolean? IncludeImages;
    }
}