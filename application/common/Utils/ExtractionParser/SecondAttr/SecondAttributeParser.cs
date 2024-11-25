using System.Text.RegularExpressions;
using Application.Common.Dto.Extraction;

namespace Application.Common.Utils.ExtractionParser.SecondAttr
{
    public class SecondAttributeParser
    {
        public static SecondAttributeProductInfo ParseResult(string input)
        {
            var result = new SecondAttributeProductInfo();


            result.SugarClaim = ParseTableSection(input, "SUGAR_CLAIM_TABLE", SecondAttributeParserDictionary.SugarClaimHeaderDict);

            result.FatClaim = ParseTableSection(input, "FAT_CLAIM_TABLE", SecondAttributeParserDictionary.FatClaimHeaderDict);

            result.ProcessClaim = ParseTableSection(input, "PROCESS_CLAIM_TABLE", SecondAttributeParserDictionary.ProcessClaimHeaderDict);

            result.CalorieClaim = ParseTableSection(input, "CALORIE_CLAIM_TABLE", SecondAttributeParserDictionary.CalorieClaimHeaderDict);

            result.SaltClaim = ParseTableSection(input, "SALT_CLAIM_TABLE", SecondAttributeParserDictionary.SaltClaimHeaderDict);

            result.FirstExtraClaim = ParseTableSection(input, "FIRST_EXTRA_CLAIM_TABLE", SecondAttributeParserDictionary.ExtraClaimHeaderDict);

            result.SecondExtraClaim = ParseTableSection(input, "SECOND_EXTRA_CLAIM_TABLE", SecondAttributeParserDictionary.ExtraClaimHeaderDict);

            result.ThirdExtraClaim = ParseTableSection(input, "THIRD_EXTRA_CLAIM_TABLE", SecondAttributeParserDictionary.ExtraClaimHeaderDict);

            return result;
        }

        private static List<Dictionary<string, string>> ParseTableSection(string input, string tableName, Dictionary<string, string> headerMapping = null)
        {
            var tableContent = GetContentBetween(input, tableName, $"END_{tableName}");

            var rows = tableContent.Split("\n").ToList();

            var headers = ParseTableRow(rows[0]);

            var result = new List<Dictionary<string, string>>();

            for (int i = 2; i < rows.Count; i++)
            {

                var rowData = ParseTableRow(rows[i]);
                var rowDict = new Dictionary<string, string>();

                for (int j = 0; j < rowData.Count; j++)
                {
                    var fieldKey = headerMapping != null && headerMapping.ContainsKey(headers[j]) ? headerMapping[headers[j]] : headers[j];
                    rowDict[fieldKey] = rowData[j];
                }

                result.Add(rowDict);
            }
            return result;
        }

        private static List<string> ParseTableRow(string rowContent)
        {
            var rowCells = rowContent.Split('|');

            return rowCells.Select(cell => cell.Trim())
                .ToList().GetRange(1, rowCells.Length - 2);
        }

        private static string GetContentBetween(string input, string startMarker, string endMarker)
        {
            var pattern = $"{startMarker}\\n(.*?)\\n{endMarker}";
            var match = Regex.Match(input, pattern, RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }
    }
}