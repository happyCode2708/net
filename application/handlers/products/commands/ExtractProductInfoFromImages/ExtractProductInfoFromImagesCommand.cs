using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using MyApi.Application.Common.Enums;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MyApi.Application.Handlers.Products.Commands.ExtractProductInfoFromImages
{
    public class ExtractProductInfoFromImagesCommand : IRequest<ResponseModel<ExtractProductInfoFromImageResponse>>
    {
        public ExtractProductInfoFromImagesRequest Request;

        public ExtractProductInfoFromImagesCommand(ExtractProductInfoFromImagesRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<ExtractProductInfoFromImagesCommand, ResponseModel<ExtractProductInfoFromImageResponse>>
        {
            private IGenerativeServices _generativeServices;

            private IApplicationDbContext _context;

            private IPromptBuilderService _promptBuilderService;

            public Handler(IGenerativeServices generativeService, IPromptBuilderService PromptBuilderService, IApplicationDbContext Context)
            {
                _generativeServices = generativeService;
                _promptBuilderService = PromptBuilderService;
                _context = Context;
            }

            public async Task<ResponseModel<ExtractProductInfoFromImageResponse>> Handle(ExtractProductInfoFromImagesCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;

                var productId = requestObject.ProductId;

                var product = await _context.Products
                    .Include(p => p.ProductImages!)
                    .ThenInclude(pi => pi.Image!)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null || product.ProductImages == null)
                {
                    return ResponseModel<ExtractProductInfoFromImageResponse>.Fail("Failed");
                }

                // Safely extract paths from non-null images
                var imagePathList = product.ProductImages
                    .Where(pi => pi.Image != null)
                    .Select(pi => pi.Image!.Path)
                    .ToList();

                var generativeOptions = new GenerativeContentOptions
                {
                    ImagePathList = imagePathList,
                    Prompt = _promptBuilderService.MakeMarkdownNutritionPrompt("", 4),
                    ModelId = GenerativeModelEnum.Gemini_1_5_Pro_002,
                };


                var result = await _generativeServices.GenerateContentAsync(generativeOptions);

                var res = new ExtractProductInfoFromImageResponse
                {
                    FullResult = result.RawResult,
                    ConcatText = result.ConcatResult,
                };

                return ResponseModel<ExtractProductInfoFromImageResponse>.Success(res, "success");

            }
        }
    }
}