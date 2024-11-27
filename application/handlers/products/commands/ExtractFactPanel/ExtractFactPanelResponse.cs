using System.Text.Json.Nodes;
using Application.Common.Dto.Extraction;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelResponse
    {
        public JsonNode? FullResult { get; set; }

        public string? ConcatText { get; set; }

        public NutritionFactData? ExtractedInfo { get; set; }
    }
}