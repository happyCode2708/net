using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.CreateProductWithImages
{
    public class CreateProductWithImagesRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}