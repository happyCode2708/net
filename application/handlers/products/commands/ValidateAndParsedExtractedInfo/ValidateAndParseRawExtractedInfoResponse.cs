
using MyApi.Application.Common.Utils;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo {
    public class ValidateAndParseRawExtractedInfoResponse {
        // public DtoExtractResult? NewParsedResult { get; set; }

        public NewParsedResult? NewParsedData { get; set; }
    
        public DateTime? CreatedAt { get; set; }
        public ExtractSourceType? SourceType { get; set; }
        
    }

    public class NewParsedResult {
        // public FirstAttributeData? FirstAttribute { get; set;}
    }

}