using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using MyApi.Application.Common.Enums;
using MyApi.Application.Common.Utils;
using MyApi.Domain.Models;


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
            private IApplicationDbContext _context;
            private IPromptBuilderService _promptBuilderServices;


            public Handler(IGenerativeServices generativeServices, IApplicationDbContext context, IPromptBuilderService promptBuilderServices)
            {
                _generativeServices = generativeServices;
                _context = context;
                _promptBuilderServices = promptBuilderServices;
            }

            public async Task<ResponseModel<ExtractFactPanelResponse>> Handle(ExtractFactPanelCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;
                var productId = requestObject.ProductId;

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
                    .Select(pi => pi.Image!.Path)
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
                    // Setup AI analysis options
                    var generativeOptions = new GenerativeContentOptions
                    {
                        ImagePathList = imagePathList,
                        Prompt = _promptBuilderServices.MakeMarkdownNutritionPrompt("", imagePathList.Count),
                        ModelId = GenerativeModelEnum.Gemini_1_5_Pro_002,
                    };

                    // Get AI analysis results
                    var result = await _generativeServices.GenerateContentAsync(generativeOptions);

                    // Parse nutrition facts from markdown response
                    var parsedNutritionData = !String.IsNullOrEmpty(result.ConcatResult) ? NutritionFactParserUtils.ParseMarkdownResponse(result.ConcatResult) : null;

                    // Update extract session with results
                    extractSession.RawExtractData = result.ConcatResult;
                    extractSession.ExtractedData = System.Text.Json.JsonSerializer.Serialize(parsedNutritionData);
                    extractSession.Status = ExtractStatus.Completed;
                    extractSession.CompletedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ExtractFactPanelResponse
                    {
                        FullResult = result.RawResult,
                        ConcatText = result.ConcatResult,
                        ParsedResult = parsedNutritionData
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