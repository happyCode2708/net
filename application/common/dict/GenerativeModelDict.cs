using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Common.Dict
{
    public static class GenerativeModelDict
    {
        public static Dictionary<GenerativeModelEnum, string> Map = new Dictionary<GenerativeModelEnum, string>
        {
            {GenerativeModelEnum.Gemini_1_5_Flash_001, "gemini-1.5-flash-001"},
            {GenerativeModelEnum.Gemini_1_5_Pro_001,  "gemini-1.5-pro-001"},
            {GenerativeModelEnum.Gemini_1_5_Flash_002, "gemini-1.5-flash-002"},
            {GenerativeModelEnum.Gemini_1_5_Pro_002,  "gemini-1.5-pro-002"}
        };
    }
}