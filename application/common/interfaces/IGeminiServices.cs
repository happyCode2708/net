using Application.Common.Dto.Gemini;


namespace MyApi.Application.Common.Interfaces
{
    public interface IGeminiServices
    {
        Task<GeminiGenerateContentResult> GenerateContentWithApiKeyAsync(GeminiGenerativeContentOptions GenerateOptions);
    }
}

