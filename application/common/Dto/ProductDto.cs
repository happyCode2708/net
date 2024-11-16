using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Utils;
using MyApi.Domain.Models;


namespace MyApi.Application.Common.Dto
{
    public class ProductImageDto
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
    }
}