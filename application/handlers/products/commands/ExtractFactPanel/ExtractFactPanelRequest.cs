using System.Text.Json.Serialization;
using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelRequest
    {
        public int ProductId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenerativeServiceTypeEnum ServiceType { get; set; } = GenerativeServiceTypeEnum.Gemini;
    }
}