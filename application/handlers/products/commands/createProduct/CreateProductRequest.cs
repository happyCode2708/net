using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.CreateProduct
{
    public class CreateProductRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
    }
}