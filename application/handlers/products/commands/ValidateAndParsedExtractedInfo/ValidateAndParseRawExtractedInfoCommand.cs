using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Utils;
using MyApi.Domain.Models;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;
using AutoMapper;
using MyApi.Application.Common.Utils.ExtractedDataValidation;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo
{
    public class ValidateAndParseRawExtractedInfoCommand : IRequest<ResponseModel<ValidateAndParseRawExtractedInfoResponse>>
    {
        public ValidateAndParseRawExtractedInfoRequest Request;

        public ValidateAndParseRawExtractedInfoCommand(ValidateAndParseRawExtractedInfoRequest request)
        {
            Request = request;
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

                if (requestObject.SourceType == null)
                {
                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Source type is required");
                }


                var lastExtractSession = await _context.ExtractSessions.Where(e => e.ProductId == requestObject.ProductId && e.SourceType == requestObject.SourceType).OrderByDescending(e => e.CompletedAt).FirstAsync();


                var rawExtractData = lastExtractSession.RawExtractData;

                if (String.IsNullOrEmpty(rawExtractData))
                {
                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Could not find raw extracted data");
                }


                if (requestObject.SourceType == ExtractSourceType.NutritionFact)
                {

                    var newParsedNutritionData = NutritionFactParserUtils.ParseMarkdownResponse(rawExtractData);

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
                        SourceType = requestObject.SourceType
                    };

                    return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Success(response);
                }

                return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Something went wrong");
            }
        }

    }
}
