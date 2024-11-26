using Application.Common.Dto.Extraction;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductSecondAttributeResponse
    {
        // public string? FullResult { get; set; }

        // public string? ConcatText { get; set; }

        public SecondAttributeProductInfo? ExtractedInfo { get; set; }
    }
}