using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Domain.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string OriginFileName { get; set; }
        public string StoredFileName { get; set; }
        public string Url { get; set; }
        public string Path { get; set; } = "";
        public ICollection<ProductImage> ProductImages { get; set; } // Navigation property for the join table
    }

}