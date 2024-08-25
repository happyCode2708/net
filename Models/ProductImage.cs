using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Models
{
    public class ProductImage
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string ImageId { get; set; }
        public Image Image { get; set; }
    }
}