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

      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      // Lấy đường dẫn từ biến môi trường
      var templatePath = Environment.GetEnvironmentVariable("NUTRITION_PROMPT_TEMPLATE_PATH");
      if (string.IsNullOrEmpty(templatePath))
      {
        throw new InvalidOperationException("NUTRITION_PROMPT_TEMPLATE_PATH environment variable is not set");
      }

      // Đọc nội dung từ file
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
    public string MakeFirstAttributePrompt(string? ocrText)
    {
      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      // Lấy đường dẫn từ biến môi trường
      var templatePath = Environment.GetEnvironmentVariable("FIRST_ATTRIBUTE_PROMPT_TEMPLATE_PATH");
      if (string.IsNullOrEmpty(templatePath))
      {
        throw new InvalidOperationException("FIRST_ATTRIBUTE_PROMPT_TEMPLATE_PATH environment variable is not set");
      }

      // Đọc nội dung từ file
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

    public string MakeSecondAttributePrompt(string? ocrText)
    {
      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      // Lấy đường dẫn từ biến môi trường
      var templatePath = Environment.GetEnvironmentVariable("SECOND_ATTRIBUTE_PROMPT_TEMPLATE_PATH");
      if (string.IsNullOrEmpty(templatePath))
      {
        throw new InvalidOperationException("SECOND_ATTRIBUTE_PROMPT_TEMPLATE_PATH environment variable is not set");
      }

      // Đọc nội dung từ file
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