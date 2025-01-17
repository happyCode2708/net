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
            private IImageServices _imageServices;
            public Handler(IGenerativeServices generativeServices, IApplicationDbContext context, IPromptBuilderService promptBuilderServices, IGeminiServices geminiServices, IImageServices imageServices)
            {
                _generativeServices = generativeServices;
                _context = context;
                _promptBuilderServices = promptBuilderServices;
                _geminiServices = geminiServices;
                _imageServices = imageServices;
            }

            public async Task<ResponseModel<ExtractProductSecondAttributeResponse>> Handle(ExtractProductSecondAttributeCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;
                var productId = requestObject.ProductId;
                var serviceType = requestObject.ServiceType.ToString();


                GenerativeServiceTypeEnum parsedServiceType;
                try
                {
                    parsedServiceType = (GenerativeServiceTypeEnum)Enum.Parse(typeof(GenerativeServiceTypeEnum), serviceType, true);
                }
                catch (Exception ex)
                {
                    return ResponseModel<ExtractProductSecondAttributeResponse>.Fail($"Invalid service type: {ex.Message}");
                }

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
                    .Select(pi => _imageServices.GetImagePath(pi.Image))
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
                    SourceType = ExtractSourceType.SecondAttribute,
                    ExtractorVersion = "1.0",
                    ProductItem = product
                };

                _context.ExtractSessions.Add(extractSession);
                await _context.SaveChangesAsync(cancellationToken);

                try
                {
                    IGenerateContentResult generatedResult;
                    var prompt = _promptBuilderServices.MakeSecondAttributePrompt(null);

                    if (parsedServiceType == GenerativeServiceTypeEnum.Gemini)
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
                            ModelId = GenerativeModelEnum.Gemini_1_5_Flash_002,
                        };
                        generatedResult = await _generativeServices.GenerateContentAsync(generativeOptions);
                    }


                    // Parse second attribute group response data
                    var parsedSecondAttributeData = !String.IsNullOrEmpty(generatedResult.ConcatResult) ? SecondAttributeParser.ParseResult(generatedResult.ConcatResult) : null;

                    // Update extract session with results
                    extractSession.RawExtractData = generatedResult.RawResult;
                    extractSession.ExtractedData = AppJson.Serialize(parsedSecondAttributeData);
                    extractSession.Status = ExtractStatus.Completed;
                    extractSession.CompletedAt = DateTime.UtcNow;
                    extractSession.ValidatedExtractedData = null; //* in progress

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ExtractProductSecondAttributeResponse
                    {
                        // FullResult = generatedResult.JsonParsedRawResult,
                        // ConcatText = generatedResult.ConcatResult,
                        ExtractedInfo = parsedSecondAttributeData
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