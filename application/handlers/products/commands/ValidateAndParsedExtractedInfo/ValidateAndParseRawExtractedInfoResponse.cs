
using Application.Common.Dto.Extraction;
using Microsoft.Identity.Client;
using MyApi.Application.Common.Utils;
using Application.Common.Utils.ExtractionParser.Nutrition;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo {
    public class ValidateAndParseRawExtractedInfoResponse {
        public ParsedAndValidatedResult ParsedAndValidatedResult { get; set; }
        public ExtractSourceType? SourceType { get; set; }        
    }

    public class ParsedAndValidatedResult {
        public NutritionFactData? NutritionFactData { get; set; }
        public ValidateNutritionFactData? ValidatedNutritionFactData { get; set; }
    }
}