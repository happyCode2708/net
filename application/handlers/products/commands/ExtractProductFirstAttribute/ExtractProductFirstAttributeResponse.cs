using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dto.Extraction;
using Newtonsoft.Json.Linq;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeResponse
    {
        public string? FullResult { get; set; }

        public string? ConcatText { get; set; }
        
        public FirstProductAttributeInfo? ParsedResult { get; set; }
        //! DEV
        public JArray? ParsedFullResult { get; set; }
    }
}