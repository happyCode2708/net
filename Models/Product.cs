using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ValidationResult { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; } // Navigation property for the join table
                                                                      // public List<ExtractSession> ExtractSessions { get; set; }
        public string? CompareResult { get; set; }
    }
}