using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyApi.application.handlers.products.queries.extractProductInfo;
using MyApi.core.controllers;
using MyApi.application.handlers.products.commands.createProduct;
using MyApi.application.handlers.products.commands.createProductWithImages;
using MyApi.application.handlers.products.commands.ExtractProductInfoFromImages;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/pim")]
    public class ProductController : BaseApiController
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
        // api api/pim/create-product
        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {

            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var result = await CommandAsync(new CreateProductCommand(request));

            return Ok(result);

        }
        // api api/pim/create-product
        [HttpPost("create-product-with-images")]
        public async Task<IActionResult> CreateProductWithImages([FromForm] CreateProductWithImagesRequest request)
        {

            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var result = await CommandAsync(new CreateProductWithImageCommand(request));

            return Ok(result);
        }
        // api api/pim/extract-product-with-images
        [HttpPost("extract-product-with-images")]
        public async Task<IActionResult> ExtractProductInfoFromImages(ExtractProductInfoFromImagesRequest request)
        {
            var result = await CommandAsync(new ExtractProductInfoFromImagesCommand(request));

            return Ok(result);
        }
    }
}