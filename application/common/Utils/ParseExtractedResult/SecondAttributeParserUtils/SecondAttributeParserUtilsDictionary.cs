namespace MyApi.Application.Common.Utils.ParseExtractedResult.SecondAttributeParserUtils { 
    public class SecondAttributeParserUtilsDictionary
    {

    // | sugar item | is item mentioned on provided images ? (answer is yes / no / unknown) ? | How product state about it ?  | do you know it through those sources of info ? (multiple sources allowed and split by ""/"") (answer are ""ingredient list"",""marketing text on product"", ""nutrition fact panel"", ""others"")| return exact sentence or phrase on provided image that prove it |

        public Dictionary<string, string> SugarClaimHeaderDict = new Dictionary<string, string> {
            { "sugar item", "sugarClaim" }
        };

        //| fat claim | does product claim that fat claim ? (answer are yes / no / unknown) (unknown when not mentioned) | do you know it through those sources of info ? (multiple sources allowed) (answer are ""ingredient list"", ""nutrition fact panel"", ""marketing text on product"", ""others"") | how do you know that ? and give me you explain(answer in string) |

        public Dictionary<string, string> FatClaimHeaderDict = new Dictionary<string, string> {
            { "fat claim", "fatClaim" }
        };

//| processing text | do the processing text present on provided images? (answer is yes / no / unknown) ? | return all texts, or sentences, or phrases indicate that(answer is ""string array"") |

        public Dictionary<string, string> ProcessClaimHeaderDict = new Dictionary<string, string> {
            { "processing text", "processingText" }
        };

//| calorie claim | does product explicitly claim the calorie claim? (answer are yes/ no / unknown) (unknown when not mentioned) | do you know it through which info ? (answer are ""ingredient list"", ""nutrition fact panel"", ""marketing text on product"", ""others"") (answer could be multiple string from many sources) |
        public Dictionary<string, string> CalorieClaimHeaderDict = new Dictionary<string, string> {
            { "calorie claim", "calorieClaim" }
        };


//| salt claim | does product explicitly claim this claim ? (answer are yes / no / unknown) (unknown when not mentioned) | do you know it through which info ? (answer are ""ingredient list"", ""nutrition fact panel"", ""marketing text on product"", ""others"") (answer could be multiple string from many sources) |
        public Dictionary<string, string> SaltClaimHeaderDict = new Dictionary<string, string> {
            { "salt claim", "saltClaim" }
        };


//| extra item | is text about item present on provided images ? (answer is yes / no / unknown) | How product state about it ? (answer are ""free from"" / ""made without"" / ""no contain"" / ""contain"" / ""free of"" / ""no"" / ""free"" / ""flavor with"" / ""other"" / ""do not use"" / ""may contain"")  |  return all texts, or sentences, or phrases indicate that(answer is ""string array"") |
        public Dictionary<string, string> ExtraClaimHeaderDict = new Dictionary<string, string> {
            { "extra item", "extraItem" }
        };

    }
}