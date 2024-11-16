using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyApi.Domain.Models;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
using MyApi.Application.Common.Interfaces;

namespace MyApi.Application.Common.Mappings.Products
{
    // public class ProductMappingExtractionResult : Profile
    // {
    //     public ProductMappingExtractionResult()
    //     {
    //         CreateMap<ProductGridItem, ProductWithImageAndExtractionResult>()
    //         .ForMember(dest => dest.ExtractionData,
    //                       opt => opt.MapFrom(src => new ProductExtractionData()));
    //     }
    // }
}