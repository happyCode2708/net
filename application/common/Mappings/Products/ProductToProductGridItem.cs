using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Domain.Models;
using AutoMapper;
using MyApi.Application.Common.Dto;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;

namespace MyApi.Application.Common.Mappings.Products
{
    public class ProductGridItemMappingImage : Profile
    {
        public ProductGridItemMappingImage()
        {

            //* Map ProductImage -> ProductImageDto [1]
            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Image.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Image.Url));

            //* Map Product -> ProductGridItem [2] (include [1])
            CreateMap<Product, ProductGridItem>();

            //* Map ProductGridItem -> ProductGridItemWithExtractionResult [3]
            CreateMap<ProductGridItem, ProductGridItemWithExtractionResult>();
        }
    }
}