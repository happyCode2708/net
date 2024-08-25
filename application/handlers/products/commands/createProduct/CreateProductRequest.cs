using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models;

namespace MyApi.application.handlers.products.commands.createProduct
{
    public class CreateProductRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public string? ValidationResult { get; set; }
        public List<ProductImage>? ProductImages { get; set; } // Navigation property for the join table
    }
}