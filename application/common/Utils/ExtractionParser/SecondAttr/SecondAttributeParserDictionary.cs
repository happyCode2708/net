namespace Application.Common.Utils.ExtractionParser.SecondAttr
{
    public static class SecondAttributeParserDictionary
    {
        public static Dictionary<string, string> SugarClaimHeaderDict = new Dictionary<string, string> {
            { "sugar item", "sugarItem" },
            { "is item mentioned on provided images ? (answer is yes / no / unknown) ?", "isMentioned" },
            { "How product state about it ?", "productStatement" },
            { "do you know it through those sources of info ? (multiple sources allowed and split by \"\"/\"\") (answer are \"\"ingredient list\"\",\"\"marketing text on product\"\", \"\"nutrition fact panel\"\", \"\"others\"\")", "infoSources" },
            { "return exact sentence or phrase on provided image that prove it", "proofText" }
        };

        public static Dictionary<string, string> FatClaimHeaderDict = new Dictionary<string, string> {
            { "fat claim", "fatClaim" },
            { "does product claim that fat claim ? (answer are yes / no / unknown) (unknown when not mentioned)", "isClaimed" },
            { "do you know it through those sources of info ? (multiple sources allowed) (answer are \"\"ingredient list\"\", \"\"nutrition fact panel\"\", \"\"marketing text on product\"\", \"\"others\"\")", "infoSources" },
            { "how do you know that ? and give me you explain(answer in string)", "explanation" }
        };

        public static Dictionary<string, string> ProcessClaimHeaderDict = new Dictionary<string, string> {
            { "processing text", "processingText" },
            { "do the processing text present on provided images? (answer is yes / no / unknown) ?", "isPresent" },
            { "return all texts, or sentences, or phrases indicate that(answer is \"string array\")", "proofTexts" }
        };

        public static Dictionary<string, string> CalorieClaimHeaderDict = new Dictionary<string, string> {
            { "calorie claim", "calorieClaim" },
            { "does product explicitly claim the calorie claim? (answer are yes/ no / unknown) (unknown when not mentioned)", "isClaimed" },
            { "do you know it through which info ? (answer are \"ingredient list\", \"nutrition fact panel\", \"marketing text on product\", \"others\") (answer could be multiple string from many sources)", "infoSources" }
        };

        public static Dictionary<string, string> SaltClaimHeaderDict = new Dictionary<string, string> {
            { "salt claim", "saltClaim" },
            { "does product explicitly claim this claim ? (answer are yes / no / unknown) (unknown when not mentioned)", "isClaimed" },
            { "do you know it through which info ? (answer are \"ingredient list\", \"nutrition fact panel\", \"marketing text on product\", \"others\") (answer could be multiple string from many sources)", "infoSources" }
        };

        public static Dictionary<string, string> ExtraClaimHeaderDict = new Dictionary<string, string> {
            { "extra item", "extraItem" },
            { "is text about item present on provided images ? (answer is yes / no / unknown)", "isPresent" },
            { "How product state about it ? (answer are \"free from\" / \"made without\" / \"no contain\" / \"contain\" / \"free of\" / \"no\" / \"free\" / \"flavor with\" / \"other\" / \"do not use\" / \"may contain\")", "productStatement" },
            { "return all texts, or sentences, or phrases indicate that(answer is \"string array\")", "proofTexts" }
        };
    }
}