using Application.Common.Dto.Extraction;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeResponse
    {
        // public string? FullResult { get; set; }

        // public string? ConcatText { get; set; }

        public FirstProductAttributeInfo? ExtractedInfo { get; set; }
    }
}