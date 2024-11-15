using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.CreateProductWithImages
{
    public class CreateProductWithImagesResponse
    {
        public int ProductId { get; set; }
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public ICollection<CreateProductWithImagesResponseProductImageDto> ProductImages { get; set; }
    }

    public class CreateProductWithImagesResponseProductImageDto
    {
        public string FileName { get; set; }

        public string FileUrl { get; set; }
    }
}