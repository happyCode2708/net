using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Handlers.Products.Commands.CreateProduct
{
    public class CreateProductResponse
    {
        public int ProductId { get; set; }
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
    }
}