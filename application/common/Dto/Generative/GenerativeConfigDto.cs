namespace Application.Common.Dto.Generative
{
    public static class ResponseMimeType
    {
        public const string TextPlain = "text/plain";
    }


    //! need to UPDATE
    public class GenerationConfigModel
    {
        public double temperature { get; set; }
        public int maxOutputTokens { get; set; }
        public double topP { get; set; }
        public int topK { get; set; }
        public string? responseMimeType { get; set; }
    }
}