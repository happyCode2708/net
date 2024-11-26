
using Application.Common.Dto.Extraction;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo
{
    public class ValidateAndParseRawExtractedInfoResponse
    {
        public ParsedAndValidatedResult ParsedAndValidatedResult { get; set; }
        public ExtractSourceType? SourceType { get; set; }
    }

    public class ParsedAndValidatedResult
    {
        public NutritionFactData? NutritionFactData { get; set; }
        public ValidateNutritionFactData? ValidatedNutritionFactData { get; set; }
        public FirstProductAttributeInfo? FirstAttributeData { get; set; }
        public ValidatedFirstAttributeData? ValidatedFirstAttributeData { get; set; }
    }
}