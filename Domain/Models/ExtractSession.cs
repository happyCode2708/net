namespace MyApi.Domain.Models
{

    public class ExtractSourceType
    {
        public static readonly ExtractSourceType NutritionFact = new("NutritionFact");
        public static readonly ExtractSourceType FirstAttribute = new("FirstAttribute");
        public static readonly ExtractSourceType SecondAttribute = new("SecondAttribute");

        public string Value { get; }

        private ExtractSourceType(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;

        public static ExtractSourceType Parse(string value)
        {
            if (value == NutritionFact.Value) return NutritionFact;
            if (value == FirstAttribute.Value) return FirstAttribute;
            if (value == SecondAttribute.Value) return SecondAttribute;
            throw new Exception($"Invalid extract source type: {value}");
        }

    }

    public class ExtractStatus
    {
        public static readonly ExtractStatus Pending = new("Pending");
        public static readonly ExtractStatus Processing = new("Processing");
        public static readonly ExtractStatus Completed = new("Completed");
        public static readonly ExtractStatus Failed = new("Failed");

        public string Value { get; }

        private ExtractStatus(string value)
        {
            Value = value;
        }
        public override string ToString() => Value;

        public static ExtractStatus Parse(string value)
        {
            if (value == Pending.Value) return Pending;
            if (value == Processing.Value) return Processing;
            if (value == Completed.Value) return Completed;
            if (value == Failed.Value) return Failed;
            throw new Exception($"Invalid extract status: {value}");
        }
    }

    public class ExtractSession
    {
        public int Id { get; set; }
        public ExtractSourceType? SourceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ExtractStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public int ProductId { get; set; }
        public Product ProductItem { get; set; }
        public string? RawExtractData { get; set; }
        public string? ExtractedData { get; set; }
        public string? ValidatedExtractedData { get; set; }
        public string ExtractorVersion { get; set; } // extractor version
    }
}