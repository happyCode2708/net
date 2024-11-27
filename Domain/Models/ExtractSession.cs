namespace MyApi.Domain.Models
{
    public enum ExtractSourceType
    {
        NutritionFact,
        FirstAttribute,
        SecondAttribute
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

        public ExtractSourceType SourceType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public ExtractStatus Status { get; set; }

        public string? ErrorMessage { get; set; }
        public int ProductId { get; set; }
        public required Product ProductItem { get; set; }
        public string? RawExtractData { get; set; }
        public string? ExtractedData { get; set; }
        public string? ValidatedExtractedData { get; set; }
        public string? ExtractorVersion { get; set; } // extractor version
    }
}