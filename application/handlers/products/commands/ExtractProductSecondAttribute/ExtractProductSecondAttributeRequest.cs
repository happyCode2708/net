using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Handlers.Products.Commands.ExtractProductFirstAttribute
{
    public class ExtractProductSecondAttributeRequest
    {
        public int ProductId { get; set; }
        public string? ServiceType { get; set; } = GenerativeDict.GetServiceType[GenerativeServiceTypeEnum.Generative];
    }
}