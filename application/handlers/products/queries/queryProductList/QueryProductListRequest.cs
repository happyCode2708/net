using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductListRequest
    {
        public string? SearchText;
        public int? PageNumber;
        public int? PageSize;
    }
}