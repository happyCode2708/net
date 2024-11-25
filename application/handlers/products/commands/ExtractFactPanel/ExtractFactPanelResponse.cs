using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dto.Extraction;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelResponse
    {
        // public string? FullResult { get; set; }

        // public string? ConcatText { get; set; }

        public NutritionFactData? ExtractedInfo { get; set; }
    }
}