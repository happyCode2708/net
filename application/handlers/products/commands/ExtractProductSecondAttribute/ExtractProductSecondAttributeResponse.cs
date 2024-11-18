using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;
using MyApi.Application.Common.Utils.ParseExtractedResult.SecondAttributeParserUtils;
using Newtonsoft.Json.Linq;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductSecondAttributeResponse
    {
        public string? FullResult { get; set; }

        public string? ConcatText { get; set; }

        public SecondAttributeProductInfo? ParsedResult { get; set; }
        //! DEV
        public JArray? ParsedFullResult { get; set; }
    }
}