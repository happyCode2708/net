using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common.Dto.Product;
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
}