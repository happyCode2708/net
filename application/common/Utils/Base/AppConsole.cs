namespace MyApi.Application.Common.Utils.Base
{
    public static class AppConsole
    {
        public static void WriteLine<T>(string name, T value)
        {
            Console.WriteLine($"{name}: {value}");
        }
        public static void WriteLineObject<T>(string name, T value)
        {
            Console.WriteLine(name);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(value, new System.Text.Json.JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
        }
    }
}