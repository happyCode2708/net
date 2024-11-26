using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductFirstAttributeRequest
    {
        public int ProductId;
        public string? ServiceType = GenerativeDict.GetServiceType[GenerativeServiceTypeEnum.Generative];
    }
}