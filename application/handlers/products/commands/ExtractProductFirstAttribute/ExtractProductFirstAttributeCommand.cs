using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using MyApi.Application.Common.Enums;
using MyApi.Domain.Models;
using Application.Common.Utils.ExtractionParser.FirstAttr;
using Application.Common.Dto.Generative;
using MyApi.Application.Common.Dict;
using Application.Common.Dto.Gemini;
using MyApi.Application.Common.Utils.Base;
using Application.Common.Dto.Extraction;
using Application.Common.Utils.ExtractionValidator;


namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeCommand : IRequest<ResponseModel<ExtractProductFirstAttributeResponse>>
    {
        public ExtractProductFirstAttributeRequest Request;
        public ExtractProductFirstAttributeCommand(ExtractProductFirstAttributeRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<ExtractProductFirstAttributeCommand, ResponseModel<ExtractProductFirstAttributeResponse>>
        {
            private IGenerativeServices _generativeServices;
            private IGeminiServices _geminiServices;
            private IApplicationDbContext _context;
            private IPromptBuilderService _promptBuilderServices;
            private IImageServices _imageServices;
            public Handler(IGenerativeServices generativeServices, IGeminiServices geminiServices, IApplicationDbContext context, IPromptBuilderService promptBuilderServices, IImageServices imageServices)
            {
                _generativeServices = generativeServices;
                _geminiServices = geminiServices;
                _context = context;
                _promptBuilderServices = promptBuilderServices;
                _imageServices = imageServices;
            }

            public async Task<ResponseModel<ExtractProductFirstAttributeResponse>> Handle(ExtractProductFirstAttributeCommand request, CancellationToken cancellationToken)
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
                    return ResponseModel<ExtractProductFirstAttributeResponse>.Fail($"Invalid service type: {ex.Message}");
                }

                // Get product with images from database
                var product = await _context.Products
                    .Include(p => p.ProductImages!)
                    .ThenInclude(pi => pi.Image!)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null || product.ProductImages == null)
                {
                    return ResponseModel<ExtractProductFirstAttributeResponse>.Fail("Failed to find product or product images");
                }

                // Extract image paths
                var imagePathList = product.ProductImages
                    .Where(pi => pi.Image != null)
                    .Select(pi => _imageServices.GetImagePath(pi.Image))
                    .ToList();

                if (!imagePathList.Any())
                {
                    return ResponseModel<ExtractProductFirstAttributeResponse>.Fail("No images found for analysis");
                }

                // Create extract session
                var extractSession = new ExtractSession
                {
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow,
                    Status = ExtractStatus.Processing,
                    SourceType = ExtractSourceType.FirstAttribute,
                    ExtractorVersion = "1.0",
                    ProductItem = product
                };

                _context.ExtractSessions.Add(extractSession);
                await _context.SaveChangesAsync(cancellationToken);

                try
                {
                    IGenerateContentResult generatedResult;

                    var prompt = _promptBuilderServices.MakeFirstAttributePrompt(null);

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
                            ModelId = GenerativeModelEnum.Gemini_1_5_Pro_002,
                        };

                        generatedResult = await _generativeServices.GenerateContentAsync(generativeOptions);
                    }


                    // Parse first attribute group response data
                    var parsedFirstAttribute = !String.IsNullOrEmpty(generatedResult.ConcatResult) ? FirstAttributeParser.ParseResult(generatedResult.ConcatResult) : null;


                    var ValidatedFirstAttributeData = parsedFirstAttribute != null
                        ? new FirstAttributeValidation().handleValidateFirstAttribute(parsedFirstAttribute)
                        : null;

                    // Update extract session with results
                    extractSession.RawExtractData = generatedResult.ConcatResult;
                    extractSession.ExtractedData = AppJson.Serialize(parsedFirstAttribute);
                    extractSession.Status = ExtractStatus.Completed;
                    extractSession.CompletedAt = DateTime.UtcNow;
                    extractSession.ValidatedExtractedData = AppJson.Serialize(ValidatedFirstAttributeData);

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ExtractProductFirstAttributeResponse
                    {
                        FullResult = generatedResult.RawResult,
                        ConcatText = generatedResult.ConcatResult,
                        ExtractedInfo = parsedFirstAttribute,
                    };

                    return ResponseModel<ExtractProductFirstAttributeResponse>.Success(response);
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