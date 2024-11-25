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
using Application.Common.Utils.ExtractionParser.Nutrition;
using AutoMapper;
using Application.Common.Dto.Gemini;
using Application.Common.Dto.Generative;
using MyApi.Application.Common.Utils.ExtractedDataValidation;
using MyApi.Application.Common.Utils.Base;
using MyApi.Application.Common.Dict;



namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelCommand : IRequest<ResponseModel<ExtractFactPanelResponse>>
    {
        public ExtractFactPanelRequest Request;
        public ExtractFactPanelCommand(ExtractFactPanelRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<ExtractFactPanelCommand, ResponseModel<ExtractFactPanelResponse>>
        {
            private IGenerativeServices _generativeServices;
            private IGeminiServices _geminiServices;
            private IApplicationDbContext _context;
            private IPromptBuilderService _promptBuilderServices;
            private IMapper _mapper;
            private ILogger<ExtractFactPanelCommand> _logger;
            private IImageServices _imageServices;
            public Handler(IGenerativeServices generativeServices, IGeminiServices geminiServices, IApplicationDbContext context, IPromptBuilderService promptBuilderServices, IMapper mapper, ILogger<ExtractFactPanelCommand> logger, IImageServices imageServices)
            {
                _generativeServices = generativeServices;
                _geminiServices = geminiServices;
                _context = context;
                _promptBuilderServices = promptBuilderServices;
                _mapper = mapper;
                _logger = logger;
                _imageServices = imageServices;
            }

            public async Task<ResponseModel<ExtractFactPanelResponse>> Handle(ExtractFactPanelCommand request, CancellationToken cancellationToken)
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
                    return ResponseModel<ExtractFactPanelResponse>.Fail("Failed to find product or product images");
                }

                // Extract image paths
                var imagePathList = product.ProductImages
                    .Where(pi => pi.Image != null)
                    .Select(pi => _imageServices.GetImagePath(pi.Image))
                    .ToList();

                if (!imagePathList.Any())
                {
                    return ResponseModel<ExtractFactPanelResponse>.Fail("No images found for analysis");
                }

                // Create extract session
                var extractSession = new ExtractSession
                {
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow,
                    Status = ExtractStatus.Processing,
                    SourceType = ExtractSourceType.NutritionFact,
                    ExtractorVersion = "1.0",
                    ProductItem = product
                };

                _context.ExtractSessions.Add(extractSession);
                await _context.SaveChangesAsync(cancellationToken);

                try
                {
                    IGenerateContentResult generatedResult;

                    var prompt = _promptBuilderServices.MakeMarkdownNutritionPrompt("", imagePathList.Count);

                    if (serviceType == GenerativeDict.GetServiceType[GenerativeServiceTypeEnum.Gemini])
                    {
                        var generativeOptions = new GeminiGenerativeContentOptions
                        {
                            ImagePathList = imagePathList,
                            Prompt = prompt,
                            ModelId = GenerativeModelEnum.Gemini_1_5_Flash_002,
                        };
                        generatedResult = await _geminiServices.GenerateContentWithApiKeyAsync(generativeOptions);
                    }
                    else
                    {
                        var generativeOptions = new GenerativeContentOptions
                        {
                            ImagePathList = imagePathList,
                            Prompt = prompt,
                            ModelId = GenerativeModelEnum.Gemini_1_5_Pro_002,
                        };
                        generatedResult = await _generativeServices.GenerateContentAsync(generativeOptions);
                    }

                    // Parse nutrition facts from response
                    var parsedNutritionData = !String.IsNullOrEmpty(generatedResult.ConcatResult) ? NutritionParser.ParseMarkdownResponse(generatedResult.ConcatResult) : null;

                    var validatedNutritionFactData = parsedNutritionData != null
                        ? new NutritionFactValidation(_mapper).handleValidateNutritionFact(parsedNutritionData)
                        : null;

                    // Update extract session with results
                    extractSession.RawExtractData = generatedResult.ConcatResult;
                    extractSession.ExtractedData = AppJson.Serialize(parsedNutritionData);
                    extractSession.Status = ExtractStatus.Completed;
                    extractSession.CompletedAt = DateTime.UtcNow;
                    extractSession.ValidatedExtractedData = validatedNutritionFactData != null
                        ? AppJson.Serialize(validatedNutritionFactData)
                        : null;

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ExtractFactPanelResponse
                    {
                        // FullResult = result.RawResult,
                        // ConcatText = result.ConcatResult,
                        ExtractedInfo = parsedNutritionData
                    };

                    return ResponseModel<ExtractFactPanelResponse>.Success(response);
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