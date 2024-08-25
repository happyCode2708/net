using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.application.common.interfaces;
using MyApi.Models;

namespace MyApi.application.infrastructure.services
{
    public class ProductServices : IProductServices
    {
        private readonly IApplicationDbContext _context;

        public ProductServices(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddImageToProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> AddProduct(Product newProductItem, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(newProductItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newProductItem;

            // throw new NotImplementedException();
        }

        public Task DeleteImageFromProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProduct(int productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(Product product, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}