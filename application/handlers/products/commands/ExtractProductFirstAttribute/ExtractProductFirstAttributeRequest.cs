using System.Text.Json.Serialization;
using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeRequest
    {
        public int ProductId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenerativeServiceTypeEnum ServiceType { get; set; } = GenerativeServiceTypeEnum.Gemini;
    }
}