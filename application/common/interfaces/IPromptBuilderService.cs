using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.application.common.interfaces
{
    public interface IPromptBuilderService
    {
        string MakeMarkdownNutritionPrompt(string OcrText, int ImageCount);
    }
}