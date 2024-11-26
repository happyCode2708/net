using System.Text.Json;
using System.Text.Json.Serialization;
using MyApi.Domain.Models;

namespace MyApi.Application.Common.Converters
{
    public class ExtractSourceTypeConverter : JsonConverter<ExtractSourceType>
    {
        public override ExtractSourceType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return null;

            return ExtractSourceType.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, ExtractSourceType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}