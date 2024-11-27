using System.Text.Json.Serialization;
using MyApi.Application.Common.Converters;
using MyApi.Domain.Models;

namespace MyApi.Application.Handlers.Products.Commands.ValidateAndParsedExtractedInfo
{
    public class ValidateAndParseRawExtractedInfoRequest
    {
        public int ProductId { get; set; }
        public string? SourceType { get; set; }
    }
}