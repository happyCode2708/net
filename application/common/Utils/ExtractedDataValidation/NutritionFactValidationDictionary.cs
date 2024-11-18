namespace MyApi.Application.Common.Utils.ExtractedDataValidation
{
    public static class NutritionFactValidationDictionary
    {
        public static readonly Dictionary<string, string> NutrientNameDictionary = new()
        {
            {"vit. a", "VITAMIN A"},
            { "vit a", "VITAMIN A"},
            { "vit. b1", "VITAMIN B1"},
            { "vit b1", "VITAMIN B1"},
            { "vit. b2", "VITAMIN B2"},
            { "vit b2", "VITAMIN B2"},
            { "vit. b3", "VITAMIN B3"},
            { "vit b3", "VITAMIN B3"},
            { "vit. b5", "VITAMIN B5"},
            { "vit b5", "VITAMIN B5"},
            { "vit. b6", "VITAMIN B6"},
            { "vit b6", "VITAMIN B6"},
            { "vit. b7", "VITAMIN B7"},
            { "vit b7", "VITAMIN B7"},
            { "vit. b9", "VITAMIN B9"},
            { "vit b9", "VITAMIN B9"},
            { "vit. b12", "VITAMIN B12"},
            { "vit b12", "VITAMIN B12"},
            { "vit. c", "VITAMIN C"},
            { "vit c", "VITAMIN C"},
            { "vit. d", "VITAMIN D"},
            { "vit d", "VITAMIN D"},
            { "vit. e", "VITAMIN E"},
            { "vit e", "VITAMIN E"},
            { "vit. k", "VITAMIN K"},
            { "vit k", "VITAMIN K"},
            { "vit.", "VITAMIN"},
            { "vit", "VITAMIN"},
            { "ca", "CALCIUM"},
            { "fe", "IRON"},
            { "mg", "MAGNESIUM"},
            { "zn", "ZINC"},
            { "na", "SODIUM"},
            { "k", "POTASSIUM"},
            { "p", "PHOSPHORUS"},
            { "cu", "COPPER"},
            { "mn", "MANGANESE"},
            { "se", "SELENIUM"},
            { "i", "IODINE"},
            { "cr", "CHROMIUM"},
            { "mo", "MOLYBDENUM"},
            { "pro", "PROTEIN"},
            { "carbs", "CARBOHYDRATES"},
            { "fat", "FAT"},
            { "sat. fat", "SATURATED FAT"},
            { "saturated", "SATURATED FAT"},
            { "trans. fat", "TRANS FAT"},
            { "trans", "TRANS FAT"},
            { "chol", "CHOLESTEROL"},
            { "cholest.", "CHOLESTEROL"},
            { "cholest", "CHOLESTEROL"},
            { "sug", "SUGARS"},
            { "fib", "FIBER"},
            { "omega-3", "OMEGA-3 FATTY ACIDS"},
            { "omega-6", "OMEGA-6 FATTY ACIDS"},
            { "ala", "ALPHA-LINOLENIC ACID"},
            { "dha", "DOCOSAHEXAENOIC ACID"},
            { "epa", "EICOSAPENTAENOIC ACID"},
            { "bcaas", "BRANCHED-CHAIN AMINO ACIDS"},
            { "cal", "CALORIES"},
            { "kcal", "KILOCALORIES"},
            { "potas.", "POTASSIUM"},
            { "potas", "POTASSIUM"},
            { "added sugars", "ADDED SUGAR"},
            { "sugar alcohols", "SUGAR ALCOHOL"},
            { "total carbohydrate", "TOTAL CARBOHYDRATES"},
            { "fiber", "DIETARY FIBER"},
            { "total carb.", "TOTAL CARBOHYDRATES"},
            { "total carb", "TOTAL CARBOHYDRATES"},
            { "monounsat. fat", "MONOUNSATURATED FAT"},
            { "polyunsat. fat", "POLYUNSATURATED FAT"},
            { "sugars", "TOTAL SUGARS"},
        };

        public static readonly Dictionary<string, string> NutrientUomDictionary = new()
        {
            { "g", "GRAM" },
            { "mg", "MILLIGRAM" },
            { "kg", "KILOGRAM" },
            { "mcg", "MICROGRAM" },
            { "oz", "OUNCE" },
            { "lb", "POUND" },
        };
    }
}

