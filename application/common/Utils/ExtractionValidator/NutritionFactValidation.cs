using Application.Common.Dto.Extraction;
using AutoMapper;
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

        public ValidateNutritionFactData handleValidateNutritionFact(NutritionFactData nutritionFactData)
        {
            var validatedNutritionFactData = _mapper.Map<ValidateNutritionFactData>(nutritionFactData);

            var validatedFactPanelList = new List<ValidatedFactPanel>();

            foreach (var factPanel in nutritionFactData.FactPanelsData)
            {
                var validatedFactPanel = handleValidatedFactPanel(factPanel);
                if (validatedFactPanel != null)
                {
                    validatedFactPanelList.Add(validatedFactPanel);
                }
            }

            validatedNutritionFactData.ValidatedFactPanels = validatedFactPanelList;

            return validatedNutritionFactData;
        }

        private ValidatedFactPanel handleValidatedFactPanel(FactPanel factPanel)
        {
            var validatedFactPanel = _mapper.Map<ValidatedFactPanel>(factPanel);

            validatedFactPanel.ValidatedNutrients = validateNutrientDescriptor(validatedFactPanel.ValidatedNutrients);
            validatedFactPanel.ValidatedNutrients = validateNutrientName(validatedFactPanel.ValidatedNutrients);
            validatedFactPanel.ValidatedNutrients = validateAmount(validatedFactPanel.ValidatedNutrients);
            validatedFactPanel.ValidatedNutrients = validateBlendIngredients(validatedFactPanel.ValidatedNutrients);
            validatedFactPanel.ValidatedNutrients = validatePercentDailyValue(validatedFactPanel.ValidatedNutrients);

            return validatedFactPanel;
        }


        private static List<ValidatedNutrient> validateNutrientDescriptor(List<ValidatedNutrient> validatedNutrients)
        {
            return validatedNutrients;
        }

        private static List<ValidatedNutrient> validateNutrientName(List<ValidatedNutrient> validatedNutrients)
        {
            var nutrientNameDictionary = NutritionFactValidationDictionary.NutrientNameDictionary;

            foreach (var nutrient in validatedNutrients)
            {
                var nutrientName = nutrient.NutrientName;

                if (nutrientName == null) continue;

                //* special case for nutrient name
                if (nutrientName.Contains("sugar added"))
                {
                    nutrient.NutrientName = "ADDED SUGAR";
                }
                else
                {
                    nutrient.NutrientName = nutrientNameDictionary.ContainsKey(nutrientName) ? nutrientNameDictionary[nutrientName] : nutrientName;
                }
            }
            return validatedNutrients;
        }


        private static List<ValidatedNutrient> validateAmount(List<ValidatedNutrient> validatedNutrients)
        {

            var uomMap = NutritionFactValidationDictionary.NutrientUomDictionary;


            foreach (var nutrient in validatedNutrients)
            {

                var amountPerServing = nutrient.AmountPerServing?.Amount?.Trim();

                if (string.IsNullOrEmpty(amountPerServing)) continue;

                if (amountPerServing.Contains("%"))
                {
                    nutrient.DailyValue = amountPerServing;

                    if (nutrient.AmountPerServing?.Amount != null)
                    {
                        nutrient.AmountPerServing.Amount = null;
                    }
                    continue;
                }

                var specialCaseMatch = Regex.Match(amountPerServing, @"(less than|<)?\s*(\d+(\.\d+)?)([a-zA-Z]+)", RegexOptions.IgnoreCase);
                if (specialCaseMatch.Success)
                {
                    var prefix = !string.IsNullOrEmpty(specialCaseMatch.Groups[1].Value)
                        ? specialCaseMatch.Groups[1].Value.Trim() + " "
                        : "";
                    var numericPart = specialCaseMatch.Groups[2].Value;
                    var uom = specialCaseMatch.Groups[4].Value.ToLower();

                    if (uomMap.ContainsKey(uom) && nutrient.AmountPerServing != null)
                    {
                        nutrient.AmountPerServing.Amount = prefix + numericPart;
                        nutrient.AmountPerServing.AnalyticalValue = float.Parse(numericPart).ToString();
                        nutrient.AmountPerServing.Uom = uomMap[uom];
                    }
                }
            }

            return validatedNutrients;
        }

        private static List<ValidatedNutrient> validateBlendIngredients(List<ValidatedNutrient> validatedNutrients)
        {
            return validatedNutrients;
        }

        private static List<ValidatedNutrient> validatePercentDailyValue(List<ValidatedNutrient> validatedNutrients)
        {
            return validatedNutrients;
        }

    }
}