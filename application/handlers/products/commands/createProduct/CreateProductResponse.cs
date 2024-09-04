using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.application.handlers.products.commands.createProduct
{
    public class CreateProductResponse
    {
        public string ProductId { get; set; }
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
    }
}