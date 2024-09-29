using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.application.common.enums;

namespace MyApi.application.common.dict
{
    public static class GenerativeModelDict
    {
        public static Dictionary<GenerativeModelEnum, string> Map = new Dictionary<GenerativeModelEnum, string>
        {
            {GenerativeModelEnum.Gemini_1_5_Flash_001, "gemini-1.5-flash-001"},
            {GenerativeModelEnum.Gemini_1_5_Pro_001,  "gemini-1.5-pro-001"}
        };
    }
}