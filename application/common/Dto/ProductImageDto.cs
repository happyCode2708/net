using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Common.Dto
{
    public class ProductImageDto
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
    }

    public class ProductWithImage
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public string? IxoneID { get; set; }
        public string? Upc12 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ValidationResult { get; set; }
        public ICollection<ProductImageDto>? ProductImages { get; set; } // Navigation property for the join table                                  // public List<ExtractSession> ExtractSessions { get; set; }
        public string? CompareResult { get; set; }
    }
}