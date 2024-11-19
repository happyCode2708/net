using AutoMapper;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;
using System.Text.RegularExpressions;

namespace MyApi.Application.Common.Utils.ExtractedDataValidation
{
    public class NutritionFactValidation
    {

        private readonly IMapper _mapper;

        public NutritionFactValidation(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ValidateNutritionFactData Validate(NutritionFactData nutritionFactData)
        {
            
            var validatedNutritionFactData = _mapper.Map<ValidateNutritionFactData>(nutritionFactData);
            
            validatedNutritionFactData.ValidatedNutrients = validateNutrientDescriptor(validatedNutritionFactData.ValidatedNutrients);
            validatedNutritionFactData.ValidatedNutrients = validateNutrientName(validatedNutritionFactData.ValidatedNutrients);
            validatedNutritionFactData.ValidatedNutrients = validateAmount(validatedNutritionFactData.ValidatedNutrients);
            validatedNutritionFactData.ValidatedNutrients = validateBlendIngredients(validatedNutritionFactData.ValidatedNutrients);
            validatedNutritionFactData.ValidatedNutrients = validatePercentDailyValue(validatedNutritionFactData.ValidatedNutrients);

            return validatedNutritionFactData;
        }


        private static List<ValidatedNutrient> validateNutrientDescriptor(List<ValidatedNutrient> validatedNutrients) {
            return validatedNutrients;   
        }

        private static List<ValidatedNutrient> validateNutrientName(List<ValidatedNutrient> validatedNutrients) {
            var nutrientNameDictionary = NutritionFactValidationDictionary.NutrientNameDictionary;
            
            foreach (var nutrient in validatedNutrients) {
                var nutrientName = nutrient.NutrientName;
                //* special case for nutrient name
                if(nutrientName.Contains("sugar added")) {
                    nutrient.NutrientName = "ADDED SUGAR";
                }else {
                    nutrient.NutrientName = nutrientNameDictionary.ContainsKey(nutrientName) ? nutrientNameDictionary[nutrientName] : nutrientName;
                }
            }
            return validatedNutrients;
        }

        
        private static List<ValidatedNutrient> validateAmount(List<ValidatedNutrient> validatedNutrients) {
 
            var uomMap = NutritionFactValidationDictionary.NutrientUomDictionary;

            foreach (var nutrient in validatedNutrients) {
                var amountPerServing = nutrient.AmountPerServing?.Amount?.Trim();

                if (string.IsNullOrEmpty(amountPerServing)) continue;

                if (amountPerServing.Contains("%")) {
                    nutrient.DailyValue = amountPerServing;

                    if(nutrient.AmountPerServing?.Amount != null) {
                        nutrient.AmountPerServing.Amount = null;
                    }
                    continue;
                }

                var specialCaseMatch = Regex.Match(amountPerServing, @"(less than|<)?\s*(\d+(\.\d+)?)([a-zA-Z]+)", RegexOptions.IgnoreCase);
                if (specialCaseMatch.Success) {
                    var prefix = !string.IsNullOrEmpty(specialCaseMatch.Groups[1].Value) 
                        ? specialCaseMatch.Groups[1].Value.Trim() + " " 
                        : "";
                    var numericPart = specialCaseMatch.Groups[2].Value;
                    var uom = specialCaseMatch.Groups[4].Value.ToLower();

                    if (uomMap.ContainsKey(uom) && nutrient.AmountPerServing != null) {
                        nutrient.AmountPerServing.Amount = prefix + numericPart;
                        nutrient.AmountPerServing.AnalyticalValue = float.Parse(numericPart).ToString();
                        nutrient.AmountPerServing.Uom = uomMap[uom];
                    }
                }
            }

            return validatedNutrients;
        }
        
        private static List<ValidatedNutrient> validateBlendIngredients(List<ValidatedNutrient> validatedNutrients) {
            return validatedNutrients;
        }

        private static List<ValidatedNutrient> validatePercentDailyValue(List<ValidatedNutrient> validatedNutrients) {
            return validatedNutrients;
        }

    }
}   



//   let amountPerServing = modifiedNutrient?.amountPerServing?.trim();

//   if (amountPerServing?.includes('%')) {
//     modifiedNutrient['dailyValue'] = amountPerServing;
//     modifiedNutrient['amountPerServing'] = '';
//     return;
//   }

//   if (!amountPerServing) return;

//   // Check for special cases like "less than 1g" or "<1g"
//   const specialCaseMatch = amountPerServing.match(
//     /(less than|<)?\s*(\d+(\.\d+)?)([a-zA-Z]+)/i
//   );
//   if (specialCaseMatch) {
//     const prefix = specialCaseMatch[1] ? specialCaseMatch[1].trim() + ' ' : '';
//     const numericPart = specialCaseMatch[2];
//     const uom = specialCaseMatch[4].toLowerCase() as keyof typeof uomMap;

//     const uomMap = {
//       g: 'GRAM',
//       mg: 'MILLIGRAM',
//       kg: 'KILOGRAM',
//       mcg: 'MICROGRAM',
//       oz: 'OUNCE',
//       lb: 'POUND',
//     };

//     const fullUOM = uomMap?.[uom];

//     // Set the amount to include the prefix (e.g., "<1" or "less than 1")
//     modifiedNutrient['amount'] = prefix + numericPart;

//     // Set the analyticalValue to the numeric part of the amount
//     modifiedNutrient['analyticalValue'] = parseFloat(numericPart);

//     modifiedNutrient['uom'] = fullUOM;
//     return;