using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Interfaces;
using System.IO;

namespace MyApi.Infrastructure.Services
{
  public class PromptBuilderService : IPromptBuilderService
  {
    public string MakeMarkdownNutritionPrompt(string? ocrText, int ImageCount)
    {
      return BuildPromptFromTemplate(ocrText, "NUTRITION_PROMPT_TEMPLATE_PATH");
    }

    public string MakeFirstAttributePrompt(string? ocrText)
    {
      return BuildPromptFromTemplate(ocrText, "FIRST_ATTRIBUTE_PROMPT_TEMPLATE_PATH");
    }

    public string MakeSecondAttributePrompt(string? ocrText)
    {
      return BuildPromptFromTemplate(ocrText, "SECOND_ATTRIBUTE_PROMPT_TEMPLATE_PATH");
    }

    private static string BuildPromptFromTemplate(string? ocrText, string templateEnvVar)
    {
      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      var templatePath = Environment.GetEnvironmentVariable(templateEnvVar);
      if (string.IsNullOrEmpty(templatePath))
      {
        throw new InvalidOperationException($"{templateEnvVar} environment variable is not set");
      }

      string promptTemplate;
      try
      {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);
        promptTemplate = File.ReadAllText(fullPath);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Could not read prompt template file: {ex.Message}");
      }

      return $@"{ocr}{promptTemplate}";
    }
  }
}