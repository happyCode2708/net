using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyApi.Application.Handlers.Products.Queries.ExtractProductInfo;
using MyApi.Core.Controllers;
using MyApi.Application.Handlers.Products.Commands.CreateProduct;
using MyApi.Application.Handlers.Products.Commands.CreateProductWithImages;
using MyApi.Application.Handlers.Products.Commands.ExtractProductInfoFromImages;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
using MyApi.Application.Handlers.Products.Commands.ExtractFactPanel;

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
        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProductList([FromQuery] string? searchText, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {

            var query = new QueryProductListRequest
            {
                SearchText = searchText,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var result = await QueryAsync(new QueryProductList(query));

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
        // api api/pim/extract-fact-panel
        [HttpPost("extract-fact-panel")]
        public async Task<IActionResult> ExtractFactPanel(ExtractFactPanelRequest request)
        {
            var result = await CommandAsync(new ExtractFactPanelCommand(request));

            return Ok(result);
        }
    }
}