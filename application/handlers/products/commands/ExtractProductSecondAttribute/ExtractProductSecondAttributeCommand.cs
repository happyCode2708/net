using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using MyApi.Application.Common.Enums;
using MyApi.Domain.Models;
using Application.Common.Utils.ExtractionParser.SecondAttr;
using Application.Common.Dto.Generative;
using MyApi.Application.Common.Dict;
using Application.Common.Dto.Gemini;
using MyApi.Application.Common.Utils.Base;


namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductSecondAttributeCommand : IRequest<ResponseModel<ExtractProductSecondAttributeResponse>>
    {
        public ExtractProductSecondAttributeRequest Request;
        public ExtractProductSecondAttributeCommand(ExtractProductSecondAttributeRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<ExtractProductSecondAttributeCommand, ResponseModel<ExtractProductSecondAttributeResponse>>
        {
            private IGenerativeServices _generativeServices;
            private IApplicationDbContext _context;
            private IPromptBuilderService _promptBuilderServices;
            private IGeminiServices _geminiServices;

            public Handler(IGenerativeServices generativeServices, IApplicationDbContext context, IPromptBuilderService promptBuilderServices, IGeminiServices geminiServices)
            {
                _generativeServices = generativeServices;
                _context = context;
                _promptBuilderServices = promptBuilderServices;
                _geminiServices = geminiServices;
            }

            public async Task<ResponseModel<ExtractProductSecondAttributeResponse>> Handle(ExtractProductSecondAttributeCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;
                var productId = requestObject.ProductId;
                var serviceType = requestObject.ServiceType;

                // Get product with images from database
                var product = await _context.Products
                    .Include(p => p.ProductImages!)
                    .ThenInclude(pi => pi.Image!)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null || product.ProductImages == null)
                {
                    return ResponseModel<ExtractProductSecondAttributeResponse>.Fail("Failed to find product or product images");
                }

                // Extract image paths
                var imagePathList = product.ProductImages
                    .Where(pi => pi.Image != null)
                    .Select(pi => pi.Image!.Path)
                    .ToList();

                if (!imagePathList.Any())
                {
                    return ResponseModel<ExtractProductSecondAttributeResponse>.Fail("No images found for analysis");
                }

                // Create extract session
                var extractSession = new ExtractSession
                {
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow,
                    Status = ExtractStatus.Processing,
                    SourceType = ExtractSourceType.ProductSecondAttribute,
                    ExtractorVersion = "1.0",
                    ProductItem = product
                };

                _context.ExtractSessions.Add(extractSession);
                await _context.SaveChangesAsync(cancellationToken);

                try
                {
                    IGenerateContentResult result;
                    var prompt = _promptBuilderServices.MakeSecondAttributePrompt(null);

                    if (serviceType == GenerativeDict.GetServiceType[GenerativeServiceTypeEnum.Gemini])
                    {
                        var generativeOptions = new GeminiGenerativeContentOptions
                        {
                            ImagePathList = imagePathList,
                            Prompt = _promptBuilderServices.MakeSecondAttributePrompt(null),
                            ModelId = GenerativeModelEnum.Gemini_1_5_Flash_002,
                        };
                        result = await _geminiServices.GenerateContentWithApiKeyAsync(generativeOptions);

                    }
                    else
                    {
                        var generativeOptions = new GenerativeContentOptions
                        {
                            ImagePathList = imagePathList,
                            Prompt = _promptBuilderServices.MakeSecondAttributePrompt(null),
                            ModelId = GenerativeModelEnum.Gemini_1_5_Flash_002,
                        };
                        result = await _generativeServices.GenerateContentAsync(generativeOptions);
                    }


                    // Parse nutrition facts from markdown response
                    var parsedSecondAttributeData = !String.IsNullOrEmpty(result.ConcatResult) ? SecondAttributeParser.ParseResult(result.ConcatResult) : null;

                    // Update extract session with results
                    extractSession.RawExtractData = result.RawResult;
                    extractSession.ExtractedData = AppJson.Serialize(parsedSecondAttributeData);
                    extractSession.Status = ExtractStatus.Completed;
                    extractSession.CompletedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ExtractProductSecondAttributeResponse
                    {
                        FullResult = result.RawResult,
                        ConcatText = result.ConcatResult,
                        ParsedResult = parsedSecondAttributeData
                    };

                    return ResponseModel<ExtractProductSecondAttributeResponse>.Success(response);
                }
                catch (Exception ex)
                {
                    extractSession.Status = ExtractStatus.Failed;
                    extractSession.ErrorMessage = ex.Message;
                    extractSession.CompletedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);

                    throw;
                }
            }
        }
    }
}