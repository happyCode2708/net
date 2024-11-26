using MyApi.Application.Common.Dict;
using MyApi.Application.Common.Enums;

namespace MyApi.Application.Handlers.Products.Commands.ExtractFactPanel
{
    public class ExtractFactPanelRequest
    {
        public int ProductId;
        public string? ServiceType = GenerativeDict.GetServiceType[GenerativeServiceTypeEnum.Generative];
    }
}