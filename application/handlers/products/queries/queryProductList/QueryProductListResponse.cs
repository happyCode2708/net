using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApi.Application.Common.Dto;
using MyApi.Domain.Models;
using static MyApi.Application.Common.Dto.GridDto;

namespace MyApi.Application.Handlers.Products.Queries.QueryProductList
{
    public class QueryProductListResponse
    {
        [JsonIgnore]
        public List<ProductWithImage> ProductList;

        public PaginationInfo Pagination;
    }
}