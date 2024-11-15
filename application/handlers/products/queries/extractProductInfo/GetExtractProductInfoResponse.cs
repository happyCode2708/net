using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Handlers.Products.Queries.ExtractProductInfo
{
    public class GetExtractProductInfoResponse
    {
        public object? Result { get; set; }

        public required string SessionId { get; set; }

        public required string Status { get; set; }
    }
}