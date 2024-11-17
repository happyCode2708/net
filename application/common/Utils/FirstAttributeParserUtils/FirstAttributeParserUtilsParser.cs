using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyApi.Application.Common.Utils.FirstAttributeParserUtils;

namespace MyApi.Application.Common.Utils.FirstAttributeParserUtils
{
    public class FirstAttributeParserUtilsParser
    {

        public static FirstProductAttributeInfo ParseResult(string input)
        {
            var result = new FirstProductAttributeInfo();

            var tableDict = new FirstAttributeParserUtilsDictionary();
            // Parse all sections
            result.StorageInstructions = ParseSection<StorageInstructions>(input, "STORAGE_INSTRUCTION");
            result.UsageInstructions = ParseSection<UsageInstructions>(input, "USAGE_INSTRUCTION");
            result.CookingInstructions = ParseSection<CookingInstructions>(input, "COOKING_INSTRUCTION_OBJECT");
            result.InformationInstructions = ParseSection<InformationInstructions>(input, "INFORMATION_INSTRUCTION");
            result.LabelingInfo = ParseTableSection(input, "LABELING_INFO_TABLE", tableDict.LabelInfoHeaderDict);
            result.LabelingInfoAnalysis = ParseTableSection(input, "LABELING_INFO_ANALYSIS_TABLE");
            result.AllergenInfo = ParseSection<AllergenInfo>(input, "ALLERGEN_OBJECT");
            result.HeaderInfo = ParseSection<HeaderInfo>(input, "HEADER_OBJECT");
            result.BaseCertifierClaims = ParseTableSection(input, "BASE_CERTIFIER_CLAIM_TABLE", tableDict.BaseCertifierClaimInfoHeaderDict);
            result.IngredientInfo = ParseTableSection(input, "INGREDIENT_TABLE", tableDict.IngredientInfoHeaderDict);
            result.MarketingInfo = ParseSection<MarketingInfo>(input, "MARKETING_OBJECT");
            result.AddressData = ParseSection<List<AddressData>>(input, "ADDRESS_DATA_OBJECT");
            result.SupplyChainInfo = ParseSection<SupplyChainInfo>(input, "SUPPLY_CHAIN_OBJECT");
            result.AttributeInfo = ParseTableSection(input, "ATTRIBUTE_TABLE", tableDict.AttributeInfoHeaderDict);

            return result;
        }
        private static T ParseSection<T>(string input, string sectionName)
        {
            var content = GetContentBetween(input, sectionName, $"END_{sectionName}");
            return !string.IsNullOrEmpty(content) ? AppJson.Deserialize<T>(content) : default;
        }

        private static List<Dictionary<string, string>> ParseTableSection(string input, string tableName, Dictionary<string, string> headerMapping = null)
        {
            var content = GetContentBetween(input, tableName, $"END_{tableName}");
            if (string.IsNullOrEmpty(content)) return new List<Dictionary<string, string>>();

            var rows = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length < 2) return new List<Dictionary<string, string>>();

            var headers = ParseTableRow(rows[0]); //* get default headers from the first row
            var result = new List<Dictionary<string, string>>();

            for (int i = 2; i < rows.Length; i++)
            {
                var rowData = ParseTableRow(rows[i]);
                var rowDict = new Dictionary<string, string>();

                for (int j = 0; j < headers.Count; j++)
                {
                    var originalHeader = headers[j];
                    var mappedHeader = headerMapping != null && headerMapping.ContainsKey(originalHeader)
                        ? headerMapping[originalHeader]
                        : originalHeader;

                    rowDict[mappedHeader] = j < rowData.Count ? rowData[j] : string.Empty;
                }

                result.Add(rowDict);
            }

            return result;
        }

        private static List<string> ParseTableRow(string row)
        {
            return row.Split('|')
                .Select(cell => cell.Trim())
                .Where(cell => !string.IsNullOrEmpty(cell))
                .ToList();
        }

        private static string GetContentBetween(string input, string startMarker, string endMarker)
        {
            var pattern = $"{startMarker}\\n(.*?)\\n{endMarker}";
            var match = Regex.Match(input, pattern, RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        // private static string RemoveSplash(string input)
        // {
        //     var removedSplash = input.Replace(@"\/", "/")
        //     .Replace(@"\""", "\"")
        //     .Replace(@"\n", "\n")
        //     .Replace(@"\r", "\r")
        //     .Replace(@"\t", "\t");

        //     return removedSplash;
        // }
    }


}