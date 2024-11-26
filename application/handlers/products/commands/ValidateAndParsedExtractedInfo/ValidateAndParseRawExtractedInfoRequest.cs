using Microsoft.OpenApi.Expressions;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo
{


    public class ValidateAndParseRawExtractedInfoRequest
    {
        public int ProductId;
        //! fix later
        public string? SourceType;
    }
}