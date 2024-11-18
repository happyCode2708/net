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

            //* [1] Map ProductImage -> ProductImageDto
            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Image.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Image.Url));

            //* [2] (SubMap [1]) Map Product -> ProductGridItem 
            CreateMap<Product, ProductGridItem>();

            //* [3] Map ProductGridItem -> ProductGridItemWithExtractionResult
            CreateMap<ProductGridItem, ProductGridItemWithExtractionResult>();

            //? product => productGridItem => productGridItemWithExtractionResult
        }
    }
}