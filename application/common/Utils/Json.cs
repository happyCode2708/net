using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Common.Utils
{
    public class Json
    {
        public static string Serialize<T>(T? value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value, new System.Text.Json.JsonSerializerOptions
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

            return System.Text.Json.JsonSerializer.Deserialize<T>(value, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}