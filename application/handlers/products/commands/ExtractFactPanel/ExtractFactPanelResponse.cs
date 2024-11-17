using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelResponse
    {
        public string? FullResult { get; set; }

        public string? ConcatText { get; set; }

        public NutritionFactData? ParsedResult { get; set; }
    }
}