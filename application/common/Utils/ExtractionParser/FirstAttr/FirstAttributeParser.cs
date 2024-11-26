using System.Text.RegularExpressions;
using Application.Common.Dto.Extraction;
using MyApi.Application.Common.Utils.Base;

namespace Application.Common.Utils.ExtractionParser.FirstAttr
{
    public class FirstAttributeParser
    {
        public static FirstProductAttributeInfo ParseResult(string input)
        {
            var result = new FirstProductAttributeInfo();

            result.StorageInstructions = ParseSection<StorageInstructions>(input, "STORAGE_INSTRUCTION");
            result.UsageInstructions = ParseSection<UsageInstructions>(input, "USAGE_INSTRUCTION");
            result.CookingInstructions = ParseSection<CookingInstructions>(input, "COOKING_INSTRUCTION_OBJECT");
            result.InformationInstructions = ParseSection<InformationInstructions>(input, "INFORMATION_INSTRUCTION");
            result.LabelingInfo = ParseTableSection(input, "LABELING_INFO_TABLE", FirstAttributeParserDictionary.LabelInfoHeaderDict);
            result.LabelingInfoAnalysis = ParseTableSection(input, "LABELING_INFO_ANALYSIS_TABLE", FirstAttributeParserDictionary.LabelInfoAnalysisHeaderDict);
            result.AllergenInfo = ParseSection<AllergenInfo>(input, "ALLERGEN_OBJECT");
            result.HeaderInfo = ParseSection<HeaderInfo>(input, "HEADER_OBJECT");
            result.BaseCertifierClaims = ParseTableSection(input, "BASE_CERTIFIER_CLAIM_TABLE", FirstAttributeParserDictionary.BaseCertifierClaimInfoHeaderDict);
            result.IngredientInfo = ParseTableSection(input, "INGREDIENT_TABLE", FirstAttributeParserDictionary.IngredientInfoHeaderDict);
            result.MarketingInfo = ParseSection<MarketingInfo>(input, "MARKETING_OBJECT");
            result.AddressData = ParseSection<List<AddressData>>(input, "ADDRESS_DATA_OBJECT");
            result.SupplyChainInfo = ParseSection<SupplyChainInfo>(input, "SUPPLY_CHAIN_OBJECT");
            result.AttributeInfo = ParseTableSection(input, "ATTRIBUTE_TABLE", FirstAttributeParserDictionary.AttributeInfoHeaderDict);

            return result;
        }
        private static T? ParseSection<T>(string input, string sectionName)
        {
            var content = GetContentBetween(input, sectionName, $"END_{sectionName}");

            T parsedContent = default;

            try
            {
                parsedContent = !string.IsNullOrEmpty(content) ? AppJson.Deserialize<T>(content) : default(T);
            }
            catch (Exception e)
            {
                AppConsole.WriteLine("parse section error", e.Message);
            }

            return parsedContent;

        }

        private static List<Dictionary<string, string>> ParseTableSection(string input, string tableName, Dictionary<string, string> headerMapping = null)
        {
            var content = GetContentBetween(input, tableName, $"END_{tableName}");

            var result = new List<Dictionary<string, string>>();


            if (string.IsNullOrEmpty(content))
            {
                return result;
            };

            var rows = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length < 2)
            {
                return result;
            };

            var headers = ParseTableRow(rows[0]); //* get default headers from the first row

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

                    var cellValue = rowData[j];

                    rowDict[mappedHeader] = cellValue;
                }

                result.Add(rowDict);
            }

            return result;
        }

        private static List<string> ParseTableRow(string row)
        {
            var rowCells = row.Split('|');

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