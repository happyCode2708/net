using System.Security.AccessControl;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

namespace MyApi.Application.Common.Utils.ExtractedDataValidation
{
    public class ValidatedNutrient: BaseNutrient
    {
        public AmountPerServingDto? AmountPerServing { get; set; }
    }

    public class AmountPerServingDto
    {
        public string? Amount { get; set; }
        public string? AnalyticalValue { get; set; }
        public string? Uom { get; set; }
    }

    public class ValidateNutritionFactData: BaseNutritionFactData
    {
        public List<ValidatedFactPanel> ValidatedFactPanels { get; set; }
    };

    public class ValidatedFactPanel: BaseFactPanel
    {
        public List<ValidatedNutrient> ValidatedNutrients { get; set; }
    }
}