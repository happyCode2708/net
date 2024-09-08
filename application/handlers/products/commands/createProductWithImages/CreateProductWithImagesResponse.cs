using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.handlers.products.commands.createProductWithImages
{
    public class CreateProductWithImagesResponse
    {
        public string ProductId { get; set; }
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