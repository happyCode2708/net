namespace Application.Common.Utils.ExtractionParser.FirstAttr
{
    public static class FirstAttributeParserDictionary
    {
        public static Dictionary<string, string> LabelInfoHeaderDict = new Dictionary<string, string>
        {
            { "label item", "labelItem"},
            { "label item type on product (answer is \"certification label\"/\"label text\"/\"other\") (if type \"other\" tell me what type you think it belong to)", "labelItemType"},
            { "label item type on product (answer is \"certification label\"/ \"label text\"/ \"other\") (if type \"other\" tell me what type you think it belong to)", "labelItemType"},
            { "what label item say ?", "labelText"}
        };

        public static Dictionary<string, string> LabelInfoAnalysisHeaderDict = new Dictionary<string, string>
        {
            {"label item", "labelItem"},
            {"do label indicate product does not contain something? (answer is yes/no)", "isContainAllergen"},
            {"what are exactly things that product say not contain from the label item (answer is \"array string\")", "mentionedNotContainItems"}
        };
        public static Dictionary<string, string> IngredientInfoHeaderDict = new Dictionary<string, string>
        {
            { "product type from nutrition panel ? (answer is \"nutrition facts\" / \"supplement facts\" / \"unknown\")", "productType"},
            { "prefix text of ingredient list (answer are \"other ingredients:\" / \"ingredients:\")", "ingredientPrefix"},
            { "ingredient statement", "ingredientStatement"},
            { "ingredient break-down list from ingredient statement (each ingredient split by \"/\")", "ingredientBreakDownList"},
            { "live and active cultures list statement", "liveAndActiveCulturesListStatement"},
            { "live and active cultures break-down list (each item split by \"/\")", "liveAndActiveCulturesBreakDownList"}
        };

        public static Dictionary<string, string> AttributeInfoHeaderDict = new Dictionary<string, string>
        {
            { "grade(answer are 'A' / 'B')", "grade"},
            { "juice percent(answer is number)", "juicePercent"}
        };

        public static Dictionary<string, string> BaseCertifierClaimInfoHeaderDict = new Dictionary<string, string>
        {
            { "claim", "claimName"},
            { "is product claim that ? (answer is yes/no/unknown)", "isClaimed"}
        };

        public static Dictionary<string, Type> FieldTypeLabelInfoAnalysisDict = new Dictionary<string, Type>
        {
            { "mentionedNotContainItems", typeof(string[])}
        };
    }
}