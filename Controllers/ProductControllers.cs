using Microsoft.AspNetCore.Mvc;
using MyApi.Core.Controllers;
using MyApi.Application.Handlers.Products.Commands.CreateProduct;
using MyApi.Application.Handlers.Products.Commands.CreateProductWithImages;
using MyApi.Application.Handlers.Products.Queries.QueryProductList;
using MyApi.Application.Handlers.Products.Commands.ExtractFactPanel;
using MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute;
using MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/pim")]
    public class ProductController : BaseApiController
    {
        // api api/pim/get-product-list
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

            var result = await CommandAsync(new CreateProductCommand(request));

            return Ok(result);
        }
        // api api/pim/create-product
        [HttpPost("create-product-with-images")]
        public async Task<IActionResult> CreateProductWithImages([FromForm] CreateProductWithImagesRequest request)
        {
            var result = await CommandAsync(new CreateProductWithImageCommand(request));

            return Ok(result);
        }
        // api api/pim/extract-fact-panel
        [HttpPost("extract-fact-panel")]
        public async Task<IActionResult> ExtractFactPanel([FromBody] ExtractFactPanelRequest request)
        {
            var result = await CommandAsync(new ExtractFactPanelCommand(request));

            return Ok(result);
        }
        // api api/pim/extract-product-first-attribute
        [HttpPost("extract-product-first-attribute")]
        public async Task<IActionResult> ExtractProductFirstAttribute(ExtractProductFirstAttributeRequest request)
        {
            var result = await CommandAsync(new ExtractProductFirstAttributeCommand(request));

            return Ok(result);
        }
        // api api/pim/extract-product-second-attribute
        [HttpPost("extract-product-second-attribute")]
        public async Task<IActionResult> ExtractProductSecondAttribute(ExtractProductSecondAttributeRequest request)
        {
            var result = await CommandAsync(new ExtractProductSecondAttributeCommand(request));

            return Ok(result);
        }

        // api api/pim/extract-product-second-attribute
        [HttpPost("validate-and-parse-raw-extracted-info")]
        public async Task<IActionResult> ValidateAndParsedRawExtractedInfo(ValidateAndParseRawExtractedInfoRequest request)
        {
            var result = await CommandAsync(new ValidateAndParseRawExtractedInfoCommand(request));

            return Ok(result);
        }
    }
}