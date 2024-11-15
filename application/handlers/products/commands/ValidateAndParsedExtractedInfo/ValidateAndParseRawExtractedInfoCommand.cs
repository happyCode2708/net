using MediatR;
using MyApi.Application.Common.Interfaces;
using MyApi.Core.Controllers;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Utils;
using MyApi.Domain.Models;

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

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel<ValidateAndParseRawExtractedInfoResponse>> Handle(ValidateAndParseRawExtractedInfoCommand request, CancellationToken cancellationToken)
            {
                var requestObject = request.Request;


                var latestExtractionOfProduct = await _context.ExtractSessions.Where(e => e.ProductId == requestObject.ProductId && e.SourceType == requestObject.SourceType).OrderByDescending(e => e.CompletedAt).FirstAsync();

                var newParsedResult = new NewParsedResult();

                // newParsedResult.NutritionFact = requestObject.SourceType == ExtractSourceType.NutritionFact ? latestExtractionOfProduct.ExtractedData;


                // if(!string.IsNullOrEmpty(latestExtractionOfProduct.RawExtractData) ) {
                //     return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Fail("Could not find raw extracted data");
                // }

                

                // // var newParsedResult = !string.IsNullOrEmpty(latestExtractionOfProduct.RawExtractData) ? NutritionFactParser.ParseMarkdownResponse(latestExtractionOfProduct.RawExtractData) : null;
                
                // if (requestObject.SourceType == ExtractSourceType.NutritionFact) {
                //     newParsedResult.NutritionFact = NutritionFactParser.ParseMarkdownResponse(latestExtractionOfProduct.RawExtractData);
                // };

                // latestExtractionOfProduct.ExtractedData = System.Text.Json.JsonSerializer.Serialize(newParsedResult);

                // await _context.SaveChangesAsync(cancellationToken);




                var response = new ValidateAndParseRawExtractedInfoResponse
                {
                    NewParsedData = null,
                    CreatedAt = latestExtractionOfProduct.CreatedAt,
                    SourceType = requestObject.SourceType
                };

                return ResponseModel<ValidateAndParseRawExtractedInfoResponse>.Success(response);
            }
        }

    }
}
