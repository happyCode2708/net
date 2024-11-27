namespace Application.Common.Dto.Extraction
{
    public class PanelHeaderInfo
    {
        public string? ServingPerContainer { get; set; }
        public string? ServingSize { get; set; }
        public string? EquivalentServingSize { get; set; }
        public string? AmountPerServingName { get; set; }
        public string? Calories { get; set; }
    }

    public class BaseNutrient
    {
        public string? NutrientName { get; set; }
        public string? Descriptor { get; set; }
        public string? ParentheticalStatement { get; set; }
        public string? DailyValue { get; set; }
        public string? BlendIngredients { get; set; }
    }

    public class BaseNutritionFactData
    {
        public Dictionary<string, string>? NutritionFactCheckers { get; set; }
    }

    public class BaseFactPanel
    {
        public required PanelHeaderInfo Header { get; set; }
        public required List<string> Footnotes { get; set; }
    }
}