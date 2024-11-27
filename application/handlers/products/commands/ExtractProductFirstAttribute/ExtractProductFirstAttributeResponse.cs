using Application.Common.Dto.Extraction;
using System.Text.Json.Nodes;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeResponse
    {
        public JsonNode? FullResult { get; set; }
        public string? ConcatText { get; set; }
        public FirstProductAttributeInfo? ExtractedInfo { get; set; }
    }
}