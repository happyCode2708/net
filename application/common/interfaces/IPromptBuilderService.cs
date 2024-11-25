namespace MyApi.Application.Common.Interfaces
{
    public interface IPromptBuilderService
    {
        // string MakeMarkdownNutritionPrompt_secret(string? OcrText, int ImageCount);

        string MakeMarkdownNutritionPrompt(string? OcrText, int ImageCount);

        // string MakeFirstAttributePrompt_secret(string? ocrText);

        string MakeFirstAttributePrompt(string? ocrText);

        // string MakeSecondAttributePrompt_secret(string? ocrText);

        string MakeSecondAttributePrompt(string? ocrText);
    }
}