using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Application.Common.Interfaces;

namespace MyApi.Application.Services
{
  public class PromptBuilderService : IPromptBuilderService
  {
    public string MakeMarkdownNutritionPrompt(string? ocrText, int ImageCount)
    {

      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      return $@"
{ocr}

VALIDATION AND FIX BUGS:
1) why you keep not separating the dual-column nutrition fact panel? please please help me always separated into multiple table of info for dual-column nutrition fact panel.
2) only give me the information visibly seen on provided images and do not assume the values.
3) only give me exact nutrition panel info that can be visibly seen from provided image. Gemini please stop assuming that product have nutrition fact panel
4) only extract nutrition info data a from nutrition panel on image.

IMPORTANT NOTES:
1) do not provide data that you cannot see it by human eyes on provided images.
2) ""added sugar""/ ""include n gram of added sugar"" is a separated nutrient (its ""nutrient name"" is ""added sugar"")
3) text ""include [number]g added sugars"" should be recorded as ""nutrient name"" = ""added sugars"" and ""amount per serving"" = ""[number]g"" with number is the amount per serving value.
4) do not provide me the info not seen on provided images
5) only provide me the info that visibly seen from provided images 
7) There are some cases to recognized an dual-column nutrition fact panel format:
+ case 1 - see a nutrition fact panel with two or more thn two ""percent daily value columns"" and ""amount per serving"" columns for different ""serving sizes"".
+ cas2 2 - see a nutrition fact panel with two or more thn two ""percent daily value columns"" and ""amount per serving"" columns for different ""amount per serving name""
Example 2: two columns of nutrition data for ""mixed"" and ""prepared"".
+ case 3 - see a nutrition fact panel with two or more than two percent daily value columns for different ""age groups""
Example 1: two columns of nutrition data for ""adult"" and ""children under 6 year olds""
+ case 4 - see a nutrition fact panel with two ""percent daily value columns"" for ""per serving"" and ""per container"".

8) A dual-column nutrition fact panel must be separated into two nutrition fact markdown tables with their own HEADER and FOOTNOTE (with their own index number as well). THIS IS COMPULSORY. And combining into single table is prohibited for dual-column nutrition fact panel.

9) Dual-column nutrition fact format could share the same ""footnote"" statement, or have different ""footnote"" statements.

10) sometimes nutrients could be put vertically and separated by • symbol and • symbol is not the nutrient symbol. They are not type of dual-column nutrition fact format. They are just simply put in dual-column display to save space.

11) footnote content is mostly about ""%Daily Value...."", or ""Not a significant source..."", or ""the % daily value..."" 
13) be careful the last ""nutrient row"" could be misread to be a part of ""footnote"". Remember ""footnote"" content usually about ""Daily value"" or ""percent daily value"" note.
14) 2 nutrition fact tables in provided image could be the same one, just from different angles of product. So you must read it as only one nutrition fact tables only.
15) example for result table must be in the order:
NUTRITION_FACT_VALIDATION_TABLE[1]
NUTRITION_FACT_TABLE[1]
HEADER_TABLE[1] 
FOOTNOTE_TABLE[1]
NUTRITION_FACT_VALIDATION_TABLE[2]
NUTRITION_FACT_TABLE[2]
HEADER_TABLE[2]
FOOTNOTE_TABLE[2]
....
DEBUG_TABLE

remember they are put in orders from low index [1] to higher index like [2] and [3]

and result must include all footer TABLE FOOTER (such as END__HEADER__TABLE[1],...) at the end of table.

16) ""Serving Size"" example 10 tablespoons (20g) => serving size is 10 tablespoons and equivalent = 20g
17) some ""Amount Per Serving name"" such as ""per serving"", ""per container"", ""for children > 18 years"", ""for adult"",...
18) all markdown tables must have its table name on top
19) [INDEX_NUMBER] is nutrition fact markdown table order(such as [1], [2])
20) some time footnote content could be displayed in multiple languages, the ""footnote content in english"" is the content in english only, and remember ""footnote content in english"" still include special symbols if they are available
21) blend ingredients could be also a list of sub - blends and each sub - blend could also display a list of sub - ingredients of those sub - blends.So you should record info of all sub - blends to a table cell of ""blend ingredients""
22) if a nutrient name spans multiple lines and includes additional information like brand names or descriptions, consider it as a single nutrient and combine the information into a single cell for 'nutrient name'
23) only give me actual info visibly by human eyes from provided product image
24) only return me the product info if you see image i provided
25) ""parenthetical statement about amount per serving"" is the descriptor for amount per serving(or nutrient quantity) only

27) do not add ingredients from ingredient list to nutrient list. Ingredient list usually start with ""ingredient:"" or ""other ingredients:""

SPECIFIC RULES:
1) ""nutrient name descriptor"" rules:
            +""nutrient name descriptor"" is descriptor of nutrient name(with text look like ""as something"", ""naturally occurring from something"", or other equivalent name of nutrient, ...)
+ ""nutrient name descriptor"" is usually text inside parentheses at the end of nutrient name.
+ nutrient could be a type of Extract so its nutrition name must contain ""Extract"".And the ""parenthetical statement about nutrient name"" will be the remaining text after word ""Extract"".
+ nutrient could be a type of Concentrate so its nutrition name must contain ""Concentrate"".And the ""parenthetical statement about nutrient name"" will be the remaining text after word ""Extract"".
+ nutrient info could have text ""standardized to something..."" so the text start with ""standardized to"" should be recorded as ""nutrient name descriptor""
Ex 1: ""Potato (young) Extract standardized to 20% multi-vitamin"" recorded as
nutrient name = Potato(young) Extract
nutrient name descriptor = standardized to 20 % multi - vitamin

Ex 2: ""Potato(young) Concentrate standardized to 20 % multi - vitamin"" recorded as
nutrient name = Potato(young) Concentrate
nutrient name descriptor = standardized to 20 % multi - vitamin

Ex 3: ""yogurt (29 billions living cultures)"" recorded as
nutrient name = yogurt
nutrient name descriptor = 29 billions living cultures

2) total sugar rules:
+total sugars do not have % Daily Value number, but it can have footnote symbol(such as **, *, +, ...) at "" % Daily Value"" column

MARKDOWN RULES:
1) do not bold nutrient name
2) all nutrients must return all markdown defined columns above
3) do not use bold or heading markdown syntax for table_name
4) content in a markdown table cell can only use<br> as line break instead of ""\n""

IMPORTANT NOTES:
1) dual - column nutrition panel must be always separated into two different markdown tables.
2) text in nutrition panel such as ""added sugar"", ""incl. 2g added sugar"", ""includes 2g added sugar"" must be recorded as a separated nutrient with ""nutrient name"" = ""added sugar""
3) do not bold any text in the return result

RESULT THAT I NEED:
Carefully examine provided images above.They are captured images of one product, and return info from provided images that meet all listed requirements and rules above with markdown tables format below

1) nutrition fact validation recorded in markdown table format below
TABLE FORMAT:
NUTRITION_FACT_VALIDATION_TABLE[index]
| question | answer |
| ------- | -------- |
| Do product image have nutrition fact panel ? | ...
END__NUTRITION__FACT__VALIDATION__TABLE[index]

2) Nutrition fact info recorded in markdown table format below

TABLE FORMAT:
NUTRITION_FACT_TABLE[index]
| Nutrient Name | nutrient name descriptor | Amount per Serving | parenthetical statement about amount per serving | % Daily Value and footnote symbol if avail | blend ingredients(if nutrient is a blend / mix) (nullable) |
| ------- | -------- | -------- | -------- | -------- | -------- |
END__NUTRITION__FACT__TABLE[index]

3) Header info of each nutrition fact recorded in a sub-table

TABLE_FORMAT:
            HEADER_TABLE[index]
Serving Per Container | Serving Size | Equivalent Serving Size | Amount Per Serving name | Calories
| ------- | -------- | -------- | -------- | -------- |
END__HEADER__TABLE[index]

4) Footnote of each nutrition fact recorded in a sub-table

TABLE_FORMAT:
FOOTNOTE_TABLE[index]
| footnote content at bottom of nutrition fact | footnote content in english only(include footnote symbol if avail) |
| ------- | -------- |
END__FOOTNOTE__TABLE[index]

5) Debug table is gemini answer recorded in table

TABLE_FORMAT:
DEBUG_TABLE
| question(question from debug list below) | gemini answer |
| ------- | -------- |
END__DEBUG__TABLE

debug list:
1) gemini answer me how many nutrition fact tables from provided images? are they the same one ?
2) are they in dual - column format? and tell me why? tell me why you create only one nutrition fact table for dual - column nutrition fact panel format ? remember dual - column label format must be recorded as multiple nutrition fact markdown tables.
";
    }

    public string MakeFirstAttributePrompt(string? ocrText)
    {

      var ocr = !String.IsNullOrEmpty(ocrText) ? $@"
      OCR texts from provided images:
      {ocrText}
      " : "";

      return $@"
{ocr}

VALIDATION AND FIX BUGS:
1) To avoid any deduction and ensure accuracy.
promt
2) Product info could contain multiple languages info. Only return provided info in english.

3) result must be in order and include all tables content below (note their formats right below TABLE FORMAT: or INFO FORMAT:)
STORAGE_INSTRUCTION
USAGE_INSTRUCTION
COOKING_INSTRUCTION_OBJECT
INFORMATION_INSTRUCTION
LABELING_INFO_TABLE
LABELING_INFO_ANALYSIS_TABLE
ALLERGEN_OBJECT
HEADER_OBJECT
BASE_CERTIFIER_CLAIM_TABLE
INGREDIENT_TABLE
MARKETING_OBJECT
ADDRESS_DATA_OBJECT
SUPPLY_CHAIN_OBJECT
ATTRIBUTE_TABLE

without any number like 1) or 2) before table names
without ``` or ``` closing tag

4) result must include all footer TEXT (such as END_SUPPLY_CHAIN_OBJECT,...) at the end of each table. 

5) do not add examples to return result. Please only return info that visibly seen from provided images.

6) ""array string"" type is array of strings
Example 1: [""pepper"", ""oil""]

RESULT THAT I NEED:
Carefully examine all text infos, all icons, all logos  from provided images and help me return output with all markdown tables format below (INFO FORMAT: or TABLE FORMAT:) remember that all provided images are captured pictured of one product only from different angles.

1) storage  instruction info recorded with format below:

IMPORTANT NOTES:
+ ""storage instruction"" are all instruction texts about how to storage product.
Example 1: ""reseal for freshness"", ""keep refrigerated"",...

INFO FORMAT:
STORAGE_INSTRUCTION
{{
  ""storage instructions"": ""str[]""
}}
END_STORAGE_INSTRUCTION

2) usage instruction info recorded with format below:

IMPORTANT NOTES:
+ ""usage instructions"" are all instruction text about how to use product excluding all ""cooking instructions"" 
Example 1: ""suggested use: 2 cups at one time.""

INFO FORMAT:
USAGE_INSTRUCTION
{{
  ""usage instructions"": ""str[]""
}}
END_USAGE_INSTRUCTION

3) cooking instruction info recorded with format below:

IMPORTANT NOTE:
+ if no instruction found just left it empty.
+ ""recipe ingredient list"" only provide list info if recipe have ingredient list info.

INFO FORMAT:
COOKING_INSTRUCTION_OBJECT
{{
  ""recipes"": [
  {{
    ""recipe name"": ""str"",
    ""recipe ingredient list"": ""str[] | null"",
    ""cooking steps"": ""str[]""
  }}
  ],
  ""all other text or paragraph about cooking info"": ""str[]""
}}
END_COOKING_INSTRUCTION_OBJECT

4) information instruction info recorded with format below:

IMPORTANT NOTES:
+ ""information instructions"" are some kind of informative instructions for consumer.
Example 1: ""See nutrition info for saturated fat""

INFO FORMAT:
INFORMATION_INSTRUCTION
{{
  ""information instructions"": ""str[]""
}}
END_INFORMATION_INSTRUCTION


5) LABELING INFO TABLE info recorded in markdown TABLE FORMAT below

IMPORTANT NOTE:
+ ""label item"" could be easily detected by some icons or logos or label text provided images.

+ ""label item"" could be ""certification label"", or ""label text"" seen on provided images that indicate some attributes of product.
Example 1: ""SOY FREE"" label text
Example 2: ""NUT FREE"" label text

+ remember when product state ""something free"" it mean product free of that thing (such as soy free, dairy free, ...)
Example 1: ""gluten free"" mean product not contain ""gluten""
Example 2: ""nuts free"" mean product not contain ""nuts""

+ each ""label item"" only present once time in only one row in the table.

TABLE FORMAT:
LABELING_INFO_TABLE
| label item | label item type on product (answer is ""certification label""/ ""label text""/ ""other"") (if type ""other"" tell me what type you think it belong to) | what label item say ? |
| -------- | ------- | -------- |
END_LABELING_INFO_TABLE

6) LABELING INFO ANALYSIS TABLE recorded in markdown TABLE FORMAT below

IMPORTANT NOTE:
+ ""label info analysis table"" is the analyzing table for all label item in the table of LABELING_INFO_TABLE.

TABLE FORMAT:
LABELING_INFO_ANALYSIS_TABLE
| label item | do label indicate product does not contain something? (answer is yes/no) | what are exactly things that product say not contain from the label item (answer is ""array string"") |
| ------- | -------- | -------- |
END_LABELING_INFO_ANALYSIS_TABLE

7) Allergen info recorded in the format below:
 
IMPORTANT NOTE:
+ tree nuts also includes ""coconut""

+ ""all statements about allergens product contain"" are the all contexts that you found on provided images about allergen info, usually start with ""contains:"", ""contain"", ""may contain"", ""may contain:"", ""allergen statement:, ... NOT due to sharing manufacturing equipments and NOT due to manufactured in same facility with other products.
""all statements about allergens product contain"" is not from ingredient list or recipe
Example 1: ""allergen statement: contains milk""
Example 2: ""may contain: milk, peanut""
Example 3: ""contain: milk, peanut""

+ ""allergens contain statement break-down list"" is the allergen ingredients list from ""all statements about allergens product contain"" and do not collect from product ingredient list.
+ ""allergens contain statement break-down list"" is a string list array (str[])
Example 1: [""oats"", ""milk""]
Example 2: [""peanut"", ""dairy"", ""tree nuts""]


+ ""statement about allergens on manufacturing equipments or from facility"" are the exact contexts that you found on provided images about allergens that said they present on the product since manufacturing equipments are also used to make other product, or in the same facility ,or shared machinery.
""statement about allergens on manufacturing equipments or from facility"" could be easily detected with statements with some texts such as ""produced in a facility ..."", ""Manufactured in facility that ... "", ""Made on equipment that process ... ""
Example 1: ""produced in a facility that uses soy, and peanut""
Example 2: ""Manufactured in facility that also processes peanut, milk""
Example 3: ""Made on equipment that process peanut""

+ ""allergens list from manufacturing equipments or from facility"" is the break-down list of all ingredients that is claim to present in facility or manufacturing equipments. Do not include ingredients that say is not present on facility or manufacturing equipment.
Example 1: ""Manufactured in a egg and milk free facility that also processes peanut, wheat products"" should be recorded as string array [""peanut"",""wheat""] since text ""in a egg and milk free facility"" mean the egg and milk is not present in facility.

+ ""exact text on images about allergens that product does not contain"" are the exact contexts that you found on provided images about allergen info, that product claim to not contain or free of or free.
example 1: ""contain no wheat, milk""
example 2: ""does not contain wheat, milk""
example 3: ""free of wheat, milk""
example 4: ""non-dairy"" text mean does not contain allergen ingredient of ""dairy""
example 5: ""no egg""
example 6: ""soy free"", ""dairy-free""

INFO FORMAT:
ALLERGEN_OBJECT
{{
  ""allergens contain"": 
  {{
    ""all statements about allergens product contain"": ""str[]""
    ""allergens contain statement break-down list"": ""str[]""
  }},
  ""allergens on equipments or in facility"":
  {{
    ""all statements about allergens on manufacturing equipments or from facility"": ""str[]""
    ""allergens list from manufacturing equipments or from facility"": ""str[]""
    ""allergens list not present in facility"": ""str[]""
  }},
  ""allergens product info state not contain"": 
  {{
    ""exact all texts or statements on images about allergens that product does not contain"": ""str[]""
    ""allergens product does not contain break-down list"": ""str[]""
  }},
  ""allergen information statements"": ""str[]""
}}
END_ALLERGEN_OBJECT

8) Header info with table format below:
IMPORTANT NOTE:
+ header table only have 1 row item so you must carefully examine the images.
+ ""primary size"" and ""secondary size"" and ""third size"" are a quantity measurement of product in there different unit of measurement. They are not info from ""serving size"" in nutrition fact panel.
Example 1: for ""WT 2.68 OZ (40g)"" should be recorded as
{{
  ""primary size"": ""2.68 OZ"",
  ""secondary size"": ""40g""
}}
Example 2: for ""32 fl oz ( 2 pt ) 946 mL"" should recorded as
{{
  ""primary size"": ""32 fl oz"",
  ""secondary size"": ""2 pt"",
  ""third size"": ""946 mL""
}}
Example 3: for ""100 capsules"" should recorded as
{{
  ""primary size"": ""100 capsules""
}}
Example 4: for ""20-4 OZ ( 60G ) TUBES / NET WT . 3 LB ( 853G )"" should recorded as
{{
  ""primary size"": ""3 LB"",
  ""secondary size"": ""853G""
}}
Example 5: for ""NET WT 1LB 2.7OZ (0.53KG)"" should recorded as
{{
  ""primary size"": ""0.53KG"",
  ""secondary size"": null
}}
because ""1LB 2.7OZ"" = ""0.53KG"" and ""1LB 2.7OZ"" have two different size uom so it is invalid to record as size value

Example 6: for ""NET WT 26 OZ (1 LB 10 OZ) 737G"" should recorded as
{{
  ""primary size"": ""26 OZ"",
  ""secondary size"": ""737G""
}}
because ""1 LB 10 OZ"" = ""26 OZ"" and ""1 LB 10 OZ"" have two different size uom so it is invalid to record as size value

Example 7: for ""8-1 OZ (37G) PACKS NET WT 8OZ (226G)"" recorded as
{{  
  ""primary size"": ""8OZ"",
  ""secondary size"": ""226G""
}}

Example 8: for ""2 pints (20g)"" recorded as
{{
  ""primary size"": ""2 pints"",
  ""secondary size"": ""20g""
}}

+ just collect size in order. If production mention three type of uom it will have third size

+ ""primary size"" must content quantity value number and its oum (same for primary size, and third size)

+ ""count"" is the count number of smaller unit inside a package, or a display shipper, or a case, or a box (such as count of servings, count of capsules, count of pills, ...).

+ ""full statement about product size"" is the whole size statement text found on product images that might includes all texts about primary size, secondary size,  third size and serving amounts if exits  but not info from nutrition panel
Ex 1: ""Net WT 9.28oz(260g) 10 cups""
Ex 2: ""16 FL OZ (472 ML)
Ex 3: ""900 CAPSULES 400 servings""
Ex 4: ""24 K-CUP PODS - 0.55 OZ (5.2)G/EA NET WT 4.44 OZ (38g)""

INFO FORMAT:
HEADER_OBJECT
{{
  ""product info"": {{
    ""product name"": ""str"",
    ""company name"": ""str"",
    ""brand name"": ""str""
  }},
  ""product size"": {{
    ""full statement about product size"": ""str"",
    ""primary size"": ""str"",
    ""secondary size"": ""str"",
    ""third size"": ""str"",
    ""count"": ""str"",
    ""count uom"": ""str""
  }}
}}
END_HEADER_OBJECT


9) Base certifier claim info with table format below:

IMPORTANT_NOTES:
+ carefully check for text or certifier logo that could indicate ""base certifier claim"" from provided image
Ex: logo U kosher found mean ""kosher claim"" = ""yes"" 

TABLE FORMAT:
BASE_CERTIFIER_CLAIM_TABLE
| claim | is product claim that ? (answer is yes/no/unknown) |
| ------- | ------- |
| bee friendly claim |
| bio-based claim |
| biodynamic claim |
| bioengineered claim |
| cbd cannabidiol / help claim |
| carbon footprint claim |
| certified b corporation |
| certified by international packaged ice association |
| cold pressure verified |
| cold pressure protected claim |
| cradle to cradle claim |
| cruelty free claim |
| diabetic friendly claim |
| eco fishery claim |
| fair trade claim |
| for life claim |
| use GMO claim |
| gmp claim |
| gluten-free claim |
| glycemic index claim |
| glyphosate residue free claim |
| grass-fed claim |
| halal claim |
| hearth healthy claim |
| Keto/Ketogenic Claim |
| Kosher Claim |
| Live and Active Culture Claim |
| Low Glycemic Claim |
| New York State Grown & Certified Claim |
| Non-GMO Claim |
| Organic Claim |
| PACA Claim |
| PASA Claim |
| Paleo Claim |
| Plant Based/Derived Claim |
| Rain Forest Alliance Claim |
| Vegan Claim |
| Vegetarian Claim |
| Viticulture Claim |
| Whole Grain Claim |
END_BASE_CERTIFIER_CLAIM_TABLE


9) Ingredient info with table format below:

IMPORTANT NOTE:
+ is the list of statements about ingredients of product (since product can have many ingredients list)

+ ""ingredient statement"" is content start right after a prefix text such as ""ingredients:"" or ""Ingredients:"" or ""INGREDIENTS:"" or ""other ingredients:"".

+ ""ingredient break-down list from ingredient statement"" is the list of ingredients in ingredient statement split by ""/"" (do not split sub-ingredients of an ingredient)
Example 1: ""Cookies ( Gluten Free Oat Flour , Organic Coconut Sugar , Sustainable Palm Oil),  Creme Filling (milk, onion) , Potato"" should be recorded as ""Cookies ( Gluten Free Oat Flour , Organic Coconut Sugar , Sustainable Palm Oil)/Creme Filling (milk, onion)/Potato""
Example 2: ""Noodle (flour, egg, water), Sauce(Tomato, water)"" should be recorded as ""Noodle (flour, egg, water)/Sauce(Tomato, water)""

+ ""product type from nutrition panel"" could be detected through nutrition panel text title which are NUTRITION FACTS or SUPPLEMENT FACTS

+ each ingredient in ingredient break-down list must be splitted by ""/"" character and NOT split by table cell

+ ""live and active cultures list statement"" is statement about list of living organisms (such as Lactobacillus bulgaricus and Streptococcus thermophilus—which convert pasteurized milk to yogurt)
Example 1: ""CONTAINS 5 LIVE AND ACTIVE CULTURES S. THERMOPHILUSB, L. RHAMNOSUS, LACTOBACILLUS LACTIS, L. BULGARICUS""
Example 2: ""CONTAINS 6 LIVE AND ACTIVE CULTURES S. THERMOPHILUSB, L. CASEI, L. BULGARICUS, L. RHAMNOSUS, LACTOBACILLUS LACTIS""

+ ""ingredient list info"" could be obscured due to crop image since the photos of product was taken from different angles. Try to merge into one ingredient list statement if they are same ingredient info.  

TABLE FORMAT:
INGREDIENT_TABLE
| product type from nutrition panel ? (answer is ""nutrition facts"" / ""supplement facts"" / ""unknown"") | prefix text of ingredient list (answer are ""other ingredients:"" / ""ingredients:"") | ingredient statement | ingredient break-down list from ingredient statement (each ingredient splitted by ""/"") | live and active cultures list statement | live and active cultures break-down list (each item splitted by ""/"")  | 
| ------- | ------- | -------- | -------- | -------- | -------- |
END_INGREDIENT_TABLE

10) Marketing info with format below:

IMPORTANT NOTES:
+ ""website link"" is website url link text visibly seen on product image.

+ ""social media methods on product images"" can only be detected through ""social media method name"" or ""social media icon/logo"".

INFO FORMAT:
MARKETING_OBJECT
{{
  ""websites"": [
    {{
      ""website link"": ""str"",
    }}
  ]
}}
END_MARKETING_OBJECT

11) address info with format below:

IMPORTANT NOTES:
+ ""address info text"" is the texts or sentences found on product images shoing adress of a place.

INFO FORMAT:
ADDRESS_DATA_OBJECT
[
  {{""address info text"": ""str""}}
]
END_ADDRESS_DATA_OBJECT

12) supply chain info with format below:

IMPORTANT NOTES:
+""statement indicate from which nation product was made in"" is text about the nation where a product was manufactured, produced, or grown.
Example 1: ""manufactured in Canada""
Example 2: ""made in Brazil or Chile""
Example 3: ""made in Brazil or Chile""
Example 4: ""produced in Brazil""
Example 5: ""product of France""

+ ""country of origin from made in statement"" are exact countries name(found on product images) found on product images where the product was manufactured, produced, or grown.
Example 1: [""Canada""]
Example 2: [""Brazil""]
Example 2: [""UK"", ""Brazil""]

+ ""full address statement"" rules:
            Example 1: ""distributed by: Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 2: ""Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 3: ""Heneiken Inc 999 SE HILL COURT, Milwaukie, ON N1R7L2 Canada""
Example 4: ""manufactured by Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 5: ""produced by Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 6: ""distributed by Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 7: ""dist.by: Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 8: ""MANUFACTURED FOR DISTRIBUTION BY: Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 9: 
""DISTRIBUTED BY: 
Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""
Example 10: ""operated by: Coca - cola 53 Cowsansview Road, ON N1R7L2, Canada""

+ ""prefix address"" is a prefix text prior of address
Example 1: ""MANUFACTURED FOR DISTRIBUTION BY:""
Example 2: ""DISTRIBUTED BY:""
Example 3: ""manufactured by""
Example 4: ""DISTRIBUTED BY:""
Example 5: ""manufactured for""

+ ""address type"" is values such as ""distributor"", ""manufacturer"", ""importer"", ""other"", ""not given"".That could be deducted from ""prefix address"".

+ ""phone number"" is the phone number near address info of ""distributor"" or ""manufacturer""
Example 1: ""(500) 867 - 4780""

INFO FORMAT:
SUPPLY_CHAIN_OBJECT
{{
  ""address and phone number info"": [
    {{
      ""prefix address"": ""str"",
      ""address type"": str, ,
      ""full address statement"": str,
      ""company name"": str,
      ""street number"": str,
      ""street name"": str,
      ""city"": str,
      ""state"": str,
      'zipCode': str,
      ""phone number"": ""str""
    }}
  ],
  ""country info"": {{
    ""statement indicate from which nation product was made in"": ""str[]""
    ""country of origin from made in statement"": ""str[]""
  }}
}}
END_SUPPLY_CHAIN_OBJECT

13) some other attribute info recorded with table format below:

TABLE FORMAT:
ATTRIBUTE_TABLE
| grade(answer are 'A' / 'B') | juice percent(answer is number) |
| ------- | ------- |
END_ATTRIBUTE_TABLE
            ";
    }
  }
}
