using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Common.Dto.Product;
using Application.Common.Interfaces;
using MyApi.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MyApi.Application.Common.Interfaces;

namespace MyApi.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductServices(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task AddImageToProduct(int productId, List<string> imageIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<GetProductGridItemReturn> GetProductGrid(GetProductGridProps getProductGridProps)
        {

            var query = _context.Products.AsQueryable();

            var productFilter = getProductGridProps.Filter;
            var productOptions = getProductGridProps.Options;

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


            // var productGridList = await query.Select(p => new ProductGridItem
            // {
            //     Id = p.Id,
            //     UniqueId = p.UniqueId,
            //     IxoneID = p.IxoneID,
            //     Upc12 = p.Upc12,
            //     CreatedAt = p.CreatedAt,
            //     ProductImages = includeImages ? p.ProductImages.Select(pi => new ProductImageDto
            //     {
            //         ImageId = pi.Image.Id,
            //         Url = pi.Image.Url,
            //     }).ToList() : new List<ProductImageDto>(),

            // }).ToListAsync();


            var productGridList = await query.Select(p => _mapper.Map<ProductGridItem>(p)).ToListAsync();

            var result = new GetProductGridItemReturn
            {
                ProductGridData = productGridList,
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