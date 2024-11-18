using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace MyApi.Application.Common.Utils.ParseExtractedResult.SecondAttributeParserUtils
{
    public class SecondAttributeParserUtilsParser
    {
        public static SecondAttributeProductInfo ParseResult(string input)
        {
            var result = new SecondAttributeProductInfo();

            var tableDict = new SecondAttributeParserUtilsDictionary();

            result.SugarClaim = ParseTableSection(input, "SUGAR_CLAIM_TABLE", tableDict.SugarClaimHeaderDict);

            result.FatClaim = ParseTableSection(input, "FAT_CLAIM_TABLE", tableDict.FatClaimHeaderDict);

            result.ProcessClaim = ParseTableSection(input, "PROCESS_CLAIM_TABLE", tableDict.ProcessClaimHeaderDict);

            result.CalorieClaim = ParseTableSection(input, "CALORIE_CLAIM_TABLE", tableDict.CalorieClaimHeaderDict);

            result.SaltClaim = ParseTableSection(input, "SALT_CLAIM_TABLE", tableDict.SaltClaimHeaderDict);

            result.FirstExtraClaim = ParseTableSection(input, "FIRST_EXTRA_CLAIM_TABLE", tableDict.ExtraClaimHeaderDict);

            result.SecondExtraClaim = ParseTableSection(input, "SECOND_EXTRA_CLAIM_TABLE", tableDict.ExtraClaimHeaderDict);

            result.ThirdExtraClaim = ParseTableSection(input, "THIRD_EXTRA_CLAIM_TABLE", tableDict.ExtraClaimHeaderDict);

            return result;
        }

        private static List<Dictionary<string, string>> ParseTableSection(string input, string tableName, Dictionary<string, string> headerMapping = null)
        {
            var tableContent = GetContentBetween(input, tableName, $"END__{tableName}");

            var rows = tableContent.Split("\n").ToList();

            var headers = ParseTableRow(rows[0]);

            var result = new List<Dictionary<string, string>>();

            for (int i = 2; i < rows.Count; i++)
            {

                var rowData = ParseTableRow(rows[i]);
                var rowDict = new Dictionary<string, string>();

                for (int j = 2; i < headers.Count; j++)
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
            return rowContent.Split('|').Select(cell =>
            {
                if (!String.IsNullOrEmpty(cell))
                {
                    return cell.Trim();
                }
                else
                {
                    return string.Empty;
                }
            }).ToList();
        }

        private static string GetContentBetween(string input, string startMarker, string endMarker)
        {
            var pattern = $"{startMarker}\\n(.*?)\\n{endMarker}";
            var match = Regex.Match(input, pattern, RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }


    }
}