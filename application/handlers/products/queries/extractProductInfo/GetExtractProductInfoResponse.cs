using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.application.handlers.products.queries.extractProductInfo
{
    public class GetExtractProductInfoResponse
    {
        public object? Result { get; set; }

        public required string SessionId { get; set; }

        public required string Status { get; set; }
    }
}