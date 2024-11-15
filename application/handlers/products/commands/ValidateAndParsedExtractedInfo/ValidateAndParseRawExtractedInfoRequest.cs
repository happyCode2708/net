using Microsoft.Identity.Client;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo {
    public class ValidateAndParseRawExtractedInfoRequest {
        public int ProductId;
        public ExtractSourceType SourceType;
    }
}