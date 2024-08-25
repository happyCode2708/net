using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IProductServices
    {
        Task<Product> AddProduct(Product product, CancellationToken cancellationToken = default);

        Task UpdateProduct(Product product, CancellationToken cancellationToken = default);

        Task DeleteProduct(int productId, CancellationToken cancellationToken = default);

        Task DeleteImageFromProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);
        Task AddImageToProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default);

    }
}