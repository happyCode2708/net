using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.application.handlers.products.commands.ExtractProductInfoFromImages
{
    public class ExtractProductInfoFromImageResponse
    {
        public string FullResult { get; set; }

        public string ConcatText { get; set; }
    }
}