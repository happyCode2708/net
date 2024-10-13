using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyApi.Models
{
    public class ProductImage
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}