using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Utils;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductSecondAttributeResponse
    {
        public string? FullResult { get; set; }

        public string? ConcatText { get; set; }

        public NutritionFactData? ParsedResult { get; set; }
    }
}