using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Common.Utils
{
    public class AppConsole
    {
        public static void WriteLine<T>(string name, T value)
        {
            Console.WriteLine($"{name}: {value}");
        }
        public static void WriteLineObject<T>(string name, T value)
        {
            Console.WriteLine("Latest Extractions Debug:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(value, new System.Text.Json.JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
        }
    }
}