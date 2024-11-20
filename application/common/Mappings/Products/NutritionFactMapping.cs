using AutoMapper;
using MyApi.Application.Common.Utils.ExtractedDataValidation;
using MyApi.Application.Common.Utils.ParseExtractedResult.NutritionFactParserUtils;

namespace MyApi.Application.Common.Mappings.Products
{
    public class NutritionFactMapping : Profile
    {
        public NutritionFactMapping()
        {
            CreateMap<Nutrient, ValidatedNutrient>()
                .ForMember(
                    dest => dest.AmountPerServing,
                    opt => opt.MapFrom(src => new AmountPerServingDto
                    {
                        Amount = src.AmountPerServing,
                        AnalyticalValue = null,
                        Uom = null
                    })
                );

            CreateMap<FactPanel, ValidatedFactPanel>()
               .ForMember(
                    dest => dest.ValidatedNutrients,
                    opt => opt.MapFrom(src => src.Nutrients)
                );;

            CreateMap<NutritionFactData, ValidateNutritionFactData>();
        }
    }
}