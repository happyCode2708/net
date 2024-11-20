namespace MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils
{
    public class BaseNutrient
    {
        public string NutrientName { get; set; }
        public string Descriptor { get; set; }
        public string AmountPerServing { get; set; }
        public string ParentheticalStatement { get; set; }
        public string DailyValue { get; set; }
        public string BlendIngredients { get; set; }
    }

    public class Nutrient: BaseNutrient
    {
        public string AmountPerServing { get; set; }
    }


    public class HeaderInfo
    {
        public string ServingPerContainer { get; set; }
        public string ServingSize { get; set; }
        public string EquivalentServingSize { get; set; }
        public string AmountPerServingName { get; set; }
        public string Calories { get; set; }
    }

    // public class NutritionFactData
    // {
    //     public bool HasNutritionPanel { get; set; }
    //     public HeaderInfo Header { get; set; }
    //     public List<Nutrient> Nutrients { get; set; }
    //     public List<string> Footnotes { get; set; }
    // }

    public class BaseNutritionFactData 
    {
        public bool HasNutritionPanel { get; set; }
    }

    public class NutritionFactData : BaseNutritionFactData
    {
        public List<FactPanel> FactPanelsData { get; set; }
    }

    public class BaseFactPanel  {
        public HeaderInfo Header { get; set; }
        public List<string> Footnotes { get; set; }
    }

    public class FactPanel : BaseFactPanel
    {
        public List<Nutrient> Nutrients { get; set; }
    }
}