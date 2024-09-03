using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyApi.application.handlers.products.queries.extractProductInfo;
using MyApi.core.controllers;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/pim")]
    public class Product : BaseApiController
    {
        [HttpGet("get-product-extraction-info")]
        // [Permissions(AppPermissions.VIEW_PRODUCTS, AppPermissions.EDIT_PRODUCT)]
        // [ProducesResponseType(typeof(GetProductIngredientsResponse), 200)]
        public async Task<IActionResult> GetProductIngredients([FromQuery] int productId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await QueryAsync(new GetExtractProductInfoQuery(productId));

            return Ok(result);
        }
    }
}