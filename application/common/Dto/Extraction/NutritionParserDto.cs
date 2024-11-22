namespace Application.Common.Dto.Extraction
{
    public class Nutrient : BaseNutrient
    {
        public string AmountPerServing { get; set; }
    }
    
    public class NutritionFactData : BaseNutritionFactData
    {
        public List<FactPanel> FactPanelsData { get; set; }
    }

    public class FactPanel : BaseFactPanel
    {
        public List<Nutrient> Nutrients { get; set; }
    }

}