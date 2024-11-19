using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{

    public static class ServiceType
    {
        public const string Gemini = "gemini";
        public const string Generative = "generative";
    }

    public class ExtractFactPanelRequest
    {
        public int ProductId;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string? ServiceType;
    }
}