using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils
{
    public class NutritionFactParserUtils
    {
        public static NutritionFactData ParseMarkdownResponse(string markdownResponse)
        {

            var result = new NutritionFactData
            {
                Header = new HeaderInfo(),
                Nutrients = new List<Nutrient>(),
                Footnotes = new List<string>()
            };

            var sections = markdownResponse.Split("END__");

            foreach (var section in sections)
            {
                if (section.Contains("NUTRITION_FACT_VALIDATION_TABLE"))
                {
                    var lines = section.Split('\n');
                    result.HasNutritionPanel = lines.Any(l => l.Contains("yes"));
                }

                if (section.Contains("NUTRITION_FACT_TABLE"))
                {
                    var lines = section.Split('\n')
                        .Skip(3) // Skip header and separator
                        .Where(l => !l.Contains("---")); // Thêm điều kiện này để bỏ qua separator row
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || !line.Contains("|")) continue;

                        if (line.Contains("Nutrient Name") || line.Contains("nutrient name descriptor")) continue;

                        var parts = line.Split('|')
                            .Select(p => p.Trim())
                            .ToArray();

                        if (parts.Length >= 6 && !string.IsNullOrWhiteSpace(parts[1]))
                        {
                            result.Nutrients.Add(new Nutrient
                            {
                                NutrientName = parts[1],
                                Descriptor = string.IsNullOrWhiteSpace(parts[2]) ? null : parts[2],
                                AmountPerServing = parts[3],
                                ParentheticalStatement = string.IsNullOrWhiteSpace(parts[4]) ? null : parts[4],
                                DailyValue = string.IsNullOrWhiteSpace(parts[5]) ? null : parts[5],
                                BlendIngredients = parts.Length > 6 && !string.IsNullOrWhiteSpace(parts[6]) ? parts[6] : null
                            });
                        }
                    }
                }

                if (section.Contains("HEADER_TABLE"))
                {
                    var headerLine = section.Split('\n')
                        .FirstOrDefault(l => l.Contains("About"));

                    if (headerLine != null)
                    {
                        var parts = headerLine.Split('|')
                            .Select(p => p.Trim())
                            .ToArray();

                        if (parts.Length >= 5)
                        {
                            result.Header = new HeaderInfo
                            {
                                ServingPerContainer = parts[1],
                                ServingSize = parts[2],
                                EquivalentServingSize = string.IsNullOrWhiteSpace(parts[3]) ? null : parts[3],
                                AmountPerServingName = parts[4],
                                Calories = parts[5]
                            };
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
                            result.Footnotes.Add(footnote);
                        }
                    }
                }
            }

            return result;
        }
    }
}
