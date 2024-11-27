using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Utils.Base;
using MyApi.Domain.Models;
using Application.Common.Utils.ExtractionParser.Nutrition;
using AutoMapper;
using MyApi.Application.Common.Utils.ExtractedDataValidation;
using Application.Common.Utils.ExtractionParser.FirstAttr;
using Application.Common.Utils.ExtractionValidator;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo
{
    public class ValidateAndParseRawExtractedInfoCommand : IRequest<ResponseModel<ValidateAndParseRawExtractedInfoResponse>>
    {
        public ValidateAndParseRawExtractedInfoRequest Request { get; }

        public ValidateAndParseRawExtractedInfoCommand(ValidateAndParseRawExtractedInfoRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public class Handler : IRequestHandler<ValidateAndParseRawExtractedInfoCommand, ResponseModel<ValidateAndParseRawExtractedInfoResponse>>
        {

            private IApplicationDbContext _context;

            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ResponseModel<ValidateAndParseRawExtractedInfoResponse>> Handle(ValidateAndParseRawExtractedInfoCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;

                var sourceType = requestObject.SourceType;

                var parsedSourceType = (ExtractSourceType)Enum.Parse(typeof(ExtractSourceType), sourceType, true);

                if (parsedSourceType == null)
                {
                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Source type is required");
                }

                var lastExtractSession = await _context.ExtractSessions.Where(e => e.ProductId == requestObject.ProductId && e.SourceType == (ExtractSourceType)Enum.Parse(typeof(ExtractSourceType), sourceType)).OrderByDescending(e => e.CompletedAt).FirstAsync();

                var rawExtractData = lastExtractSession.RawExtractData;

                if (String.IsNullOrEmpty(rawExtractData))
                {
                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Could not find raw extracted data");
                }


                if (parsedSourceType == ExtractSourceType.NutritionFact)
                {

                    var newParsedNutritionData = NutritionParser.ParseMarkdownResponse(rawExtractData);

                    var nutritionValidator = new NutritionFactValidation(_mapper);

                    var validatedNutritionData = nutritionValidator.handleValidateNutritionFact(newParsedNutritionData);

                    lastExtractSession.ExtractedData = AppJson.Serialize(newParsedNutritionData);
                    lastExtractSession.ValidatedExtractedData = AppJson.Serialize(validatedNutritionData);

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ValidateAndParseRawExtractedInfoResponse
                    {
                        ParsedAndValidatedResult = new ParsedAndValidatedResult
                        {
                            NutritionFactData = newParsedNutritionData,
                            ValidatedNutritionFactData = validatedNutritionData
                        },
                        SourceType = sourceType
                    };

                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Success(response);
                }


                if (parsedSourceType == ExtractSourceType.FirstAttribute)
                {
                    var newParsedFirstAttributeData = FirstAttributeParser.ParseResult(rawExtractData);

                    var firstAttributeValidator = new FirstAttributeValidation();
                    var validatedFirstAttributeData = firstAttributeValidator.handleValidateFirstAttribute(newParsedFirstAttributeData);

                    //* update to current session
                    lastExtractSession.ExtractedData = AppJson.Serialize(newParsedFirstAttributeData);
                    lastExtractSession.ValidatedExtractedData = AppJson.Serialize(validatedFirstAttributeData);

                    await _context.SaveChangesAsync(cancellationToken);

                    var response = new ValidateAndParseRawExtractedInfoResponse
                    {
                        ParsedAndValidatedResult = new ParsedAndValidatedResult
                        {
                            FirstAttributeData = newParsedFirstAttributeData,
                            ValidatedFirstAttributeData = validatedFirstAttributeData
                        },
                        SourceType = sourceType
                    };

                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Success(response);
                }

                return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Something went wrong");
            }
        }

    }
}
