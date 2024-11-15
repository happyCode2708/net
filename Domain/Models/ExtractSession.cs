using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Domain.Models
{
    public enum ExtractSourceType
    {
        NutritionFact,
        Attribute1, 
        Attribute2
    }

    public enum ExtractStatus
    {
        Pending,
        Processing, 
        Completed,
        Failed
    }

    public class ExtractSession
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ExtractStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public int ProductId { get; set; }
        public Product ProductItem { get; set; }

        public string ExtractedData { get; set; }
        public string RawExtractData {get;set;}
        public ExtractSourceType SourceType { get; set; }

        public string ExtractorVersion { get; set; } // extractor version
    }
}