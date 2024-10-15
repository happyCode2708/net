using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using MyApi.application.common.enums;
using MyApi.application.common.interfaces;
using MyApi.core.controllers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MyApi.application.handlers.products.commands.ExtractProductInfoFromImages
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


                // var product = await _context.Products.Include(p => p.ProductImages!).ThenInclude(pi => pi.Image!).FirstOrDefaultAsync(p => p.Id == productId);

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

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // For pretty JSON output
                                          // ReferenceHandler = ReferenceHandler.Preserve // Handle circular references
                };

                string json = JsonSerializer.Serialize(imagePathList, options);
                Console.WriteLine(json); // Log to console or wherever you need



                // // Serialize the images list to JSON
                // var json = JsonSerializer.Serialize(imageList, new JsonSerializerOptions
                // {
                //     WriteIndented = true, // Pretty-print the JSON
                //     ReferenceHandler = ReferenceHandler.Preserve
                // });

                // Console.WriteLine(json); // Log it to the console


                // Console.WriteLine($"test : {imageList}");

                var generativeOptions = new GenerativeContentOptions
                {
                    ImagePathList = imagePathList,
                    Prompt = _promptBuilderService.MakeMarkdownNutritionPrompt("", 4),
                    ModelId = GenerativeModelEnum.Gemini_1_5_Flash_001,
                };

                // try
                // {
                var result = await _generativeServices.GenerateContentAsync(generativeOptions);

                //* get concat text results

                JArray resultArray = JArray.Parse(result);

                var ConcatResult = String.Join(" ", resultArray.Select(r => r["candidates"]?.First?["content"]?["parts"]?.First?["text"]));

                var res = new ExtractProductInfoFromImageResponse
                {
                    FullResult = result,
                    ConcatText = ConcatResult,
                };

                return ResponseModel<ExtractProductInfoFromImageResponse>.Success(res, "success");
                // }
                // catch (HttpRequestException ex)
                // {
                //     Conwole.log
                //     throw new NotImplementedException();
                // }
            }
        }
    }
}