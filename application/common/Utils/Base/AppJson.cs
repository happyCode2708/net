using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace MyApi.Application.Common.Utils.Base
{
    public static class AppJson
    {
        public static string Serialize<T>(T? value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        public static T? Deserialize<T>(string? value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return default;
            }

            try
            {
                // Xử lý cho Newtonsoft.Json types
                if (typeof(T) == typeof(JObject))
                {
                    return (T)(object)JObject.Parse(value);
                }
                if (typeof(T) == typeof(JArray))
                {
                    return (T)(object)JArray.Parse(value);
                }
                if (typeof(T) == typeof(JToken))
                {
                    return (T)(object)JToken.Parse(value);
                }

                return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                });
            }
            catch (Exception ex)
            {
                var truncatedValue = value?.Length > 100 ? value.Substring(0, 100) + "..." : value;
                throw new JsonException($"Error deserializing to type {typeof(T).Name}. Input JSON: {truncatedValue}. Error: {ex.Message}", ex);
            }
        }
    }
}
