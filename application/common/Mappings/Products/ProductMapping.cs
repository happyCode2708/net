using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Domain.Models;
using AutoMapper;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
using Application.Common.Dto.Product;
using MyApi.Application.Common.Interfaces;

namespace MyApi.Application.Common.Mappings.Products
{
    public class ProductGridItemMappingImage : Profile
    {
        private readonly IImageServices _imageServices;
        public ProductGridItemMappingImage(IImageServices imageServices)
        {
            _imageServices = imageServices;

            //* [1] Map ProductImage -> ProductImageDto
            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Image.Id))
                .ForMember(
                    dest => dest.Url,
                    opt => opt.MapFrom((src, _, _, context) => _imageServices.GetImageUrl(src.Image))
                );

            //* [2] (SubMap [1]) Map Product -> ProductGridItem 
            CreateMap<Product, ProductGridItem>();

            //* [3] Map ProductGridItem -> ProductGridItemWithExtractionResult
            CreateMap<ProductGridItem, ProductGridItemWithExtractionResult>();

            //? product => productGridItem => productGridItemWithExtractionResult
        }
    }

    // public class ImageUrlResolver : IValueResolver<ProductImage, ProductImageDto, string>
    // {
    //     private readonly IImageServices _imageServices;

    //     public ImageUrlResolver(IImageServices imageServices)
    //     {
    //         _imageServices = imageServices;
    //     }

    //     public string Resolve(ProductImage source, ProductImageDto destination, string destMember, ResolutionContext context)
    //     {
    //         return _imageServices.GetImageUrl(source.Image);
    //     }
    // }
}