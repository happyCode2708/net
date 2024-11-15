using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductInfoFromImages
{
    public class ExtractProductInfoFromImagesRequest
    {
        public int ProductId { get; set; }
    }
}