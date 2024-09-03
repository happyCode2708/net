using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Models
{
        public class Image
        {
                public string Id { get; set; } = Guid.NewGuid().ToString();
                public string Url { get; set; }
                public string Path { get; set; } = "";
                public List<ProductImage> ProductImages { get; set; } // Navigation property for the join table
        }

}