using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dto.Gemini;
using Application.Common.Interfaces;
using MyApi.Application.Common.Enums;
using MyApi.Domain.Models;
using Newtonsoft.Json.Linq;

namespace MyApi.Application.Common.Interfaces
{
    public interface IGeminiServices
    {
        Task<GeminiGenerateContentResult> GenerateContentWithApiKeyAsync(GeminiGenerativeContentOptions GenerateOptions);
    }
}

