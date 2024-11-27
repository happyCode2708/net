namespace MyApi.Application.Common.Interfaces
{
    public interface IPromptBuilderService
    {
        string MakeMarkdownNutritionPrompt(string? OcrText, int ImageCount);
        string MakeFirstAttributePrompt(string? ocrText);
        string MakeSecondAttributePrompt(string? ocrText);
    }
}