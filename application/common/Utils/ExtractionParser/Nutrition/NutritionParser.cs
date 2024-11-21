using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Common.Dto.Extraction;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.ObjectPool;
using MyApi.Application.Common.Utils.Base;

namespace Application.Common.Utils.ExtractionParser.Nutrition
{
    public class NutritionParser
    {
        public static NutritionFactData ParseMarkdownResponse(string markdownResponse)
        {

            var result = new NutritionFactData()
            {
                FactPanelsData = new List<FactPanel>()
            };

            var sections = markdownResponse.Split("END_");

            AppConsole.WriteLineObject("sections",sections);

            int panelIndex = 0;

            foreach (var section in sections)
            {
                if (section.Contains("NUTRITION_FACT_VALIDATION_TABLE"))
                {
                    result.NutritionFactCheckers = ParseTableVertical(section, NutritionParserDictionary.NutritionFactCheckerDict);
                }

                if (section.Contains("NUTRITION_FACT_TABLE"))
                {
                    panelIndex = getIndexFromSection(section);

                    while (result.FactPanelsData.Count < panelIndex)
                    {
                        result.FactPanelsData.Add(new FactPanel
                        {
                            Nutrients = new List<Nutrient>(),
                            Footnotes = new List<string>()
                        });
                    }

                    var lines = section.Split('\n')
                        .Skip(3) // Skip header and separator
                        .Where(l => !l.Contains("---")); 
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || !line.Contains("|")) continue;

                        if (line.Contains("Nutrient Name") || line.Contains("nutrient name descriptor")) continue;

                        var parts = line.Split('|')
                            .Where(p => p != "")
                            .Select(p => p.Trim())
                            .ToArray();

                        if (parts.Length >= 6)
                        {
                            result.FactPanelsData[panelIndex - 1].Nutrients.Add(new Nutrient
                            {
                                NutrientName = parts[0],
                                Descriptor = string.IsNullOrWhiteSpace(parts[1]) ? null : parts[1],
                                AmountPerServing = parts[2],
                                ParentheticalStatement = string.IsNullOrWhiteSpace(parts[3]) ? null : parts[3],
                                DailyValue = string.IsNullOrWhiteSpace(parts[4]) ? null : parts[4],
                                BlendIngredients = parts.Length > 5 && !string.IsNullOrWhiteSpace(parts[5]) ? parts[5] : null
                            });
                        }
                    }
                }

                if (section.Contains("HEADER_TABLE"))
                {
                    var lines = section.Split('\n').Skip(3).Where(l => !l.Contains("---")).ToList();


                    if (lines.Count > 0)
                    {
                        var isLineDataValid = false;

                        foreach (var line in lines)
                        {

                            if (line.Contains("Equivalent Serving Size")) {
                                isLineDataValid = true;
                                continue;
                            };

                            if(!isLineDataValid) continue;

                            var parts = line
                                .Split('|')
                                .Where(p => p != "")
                                .Select(p => p.Trim())
                                .ToArray();

                            if (parts.Length >= 5)
                            {
                                result.FactPanelsData[panelIndex - 1].Header = new PanelHeaderInfo
                                {
                                    ServingPerContainer = parts[0],
                                    ServingSize = parts[1],
                                    EquivalentServingSize = string.IsNullOrWhiteSpace(parts[2]) ? null : parts[3],
                                    AmountPerServingName = parts[3],
                                    Calories = parts[4]
                                };
                            }
                        }
                    }
                }

                if (section.Contains("FOOTNOTE_TABLE"))
                {
                    var lines = section.Split('\n');
                    foreach (var line in lines)
                    {
                        if (!line.Contains("|") || line.Contains("---") ||
                            line.Contains("footnote content")) continue;

                        var footnote = line.Split('|')[1].Trim();
                        if (!string.IsNullOrWhiteSpace(footnote))
                        {
                            result.FactPanelsData[panelIndex - 1].Footnotes.Add(footnote);
                        }
                    }
                }
            }

            return result;
        }

        private static int getIndexFromSection(string section)
        {
            var regex = new Regex(@"NUTRITION_FACT_TABLE\[(\d+)\]");
            var match = regex.Match(section);

            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        private static Dictionary<string, string> ParseTableVertical(string tableContent, Dictionary<string, string> keyMapping = null) {
            var rows = tableContent.Split('\n').Where(r => !string.IsNullOrEmpty(r)).ToArray();

            var result = new Dictionary<string, string>();

            var startWriteRowData = false;

            foreach (var row in rows) {
                if(row.Contains("---") && row.Contains('|')) {
                    startWriteRowData = true;
                    continue;
                }

                if(!startWriteRowData) continue;
                
                var rowContent = ParseTableRow(row);

                for (int i = 1; i < rowContent.Count; i++) {
                    var originalFieldKey = rowContent[0];


                    var fieldKey = keyMapping != null && keyMapping.ContainsKey(originalFieldKey) ? keyMapping[originalFieldKey] : originalFieldKey;
                    
                    result[fieldKey] = rowContent[i];
                }
            }

            return result;
        }

         private static List<string> ParseTableRow(string row)
        {
            return  row.Split('|')
                .Where(cell => !string.IsNullOrEmpty(cell))
                .Select(cell => cell.Trim())
                .ToList();
        }
    }
}