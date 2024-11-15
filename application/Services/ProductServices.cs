using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Dto;
using MyApi.Application.Common.Interfaces;
using MyApi.Domain.Models;

namespace MyApi.Application.Services
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

        public async Task<GetProductListReturn> GetProductList(GetProductListProps getProductListProps)
        {

            var query = _context.Products.AsQueryable();

            var productFilter = getProductListProps.Filter;
            var productOptions = getProductListProps.Options;

            var searchText = productFilter.SearchText;
            var pageSize = productFilter.PageSize;
            var pageNumber = productFilter.PageNumber;

            var includeImages = productOptions?.IncludeImages ?? false;

            if (includeImages)
            {
                query = query.Include(p => p.ProductImages).ThenInclude(pi => pi.Image);
            }


            if (!String.IsNullOrEmpty(productFilter.SearchText))
            {

                query = query.Where(p => p.IxoneID.Contains(searchText) || p.Upc12.Contains(searchText) || p.Id.ToString().Contains(searchText));
            }

            //* get totalCount;
            var totalCount = query.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }


            var productList = await query.Select(p => new ProductWithImageAndExtraction
            {
                Id = p.Id,
                UniqueId = p.UniqueId,
                IxoneID = p.IxoneID,
                Upc12 = p.Upc12,
                CreatedAt = p.CreatedAt,
                ProductImages = includeImages ? p.ProductImages.Select(pi => new ProductImageDto
                {
                    ImageId = pi.Image.Id,
                    Url = pi.Image.Url,
                }).ToList() : new List<ProductImageDto>(),

            }).ToListAsync();

            var result = new GetProductListReturn
            {
                ProductList = productList,
                TotalCount = totalCount,
            };

            return result;

        }

        public async Task<Product> AddProduct(Product newProductItem, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(newProductItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newProductItem;
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