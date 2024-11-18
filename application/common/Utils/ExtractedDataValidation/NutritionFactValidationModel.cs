using System.Security.AccessControl;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

namespace MyApi.Application.Common.Utils.ExtractedDataValidation
{
    public class ValidatedNutrient
    {
        public string NutrientName { get; set; }
        public string Descriptor { get; set; }
        public AmountPerServingDto? AmountPerServing { get; set; }
        public string ParentheticalStatement { get; set; }
        public string DailyValue { get; set; }
        public string BlendIngredients { get; set; }
    }

    public class AmountPerServingDto
    {
        public string? Amount { get; set; }
        public string? AnalyticalValue { get; set; }
        public string? Uom { get; set; }
    }

    public class ValidateNutritionFactData
    {
        public bool HasNutritionPanel { get; set; }
        public HeaderInfo Header { get; set; }
        public List<ValidatedNutrient> ValidatedNutrients { get; set; }
        public List<string> Footnotes { get; set; }
    };


}