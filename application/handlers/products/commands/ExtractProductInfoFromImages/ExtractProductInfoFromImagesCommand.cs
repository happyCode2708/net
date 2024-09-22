using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MyApi.application.common.interfaces;
using MyApi.core.controllers;

namespace MyApi.application.handlers.products.commands.ExtractProductInfoFromImages
{
    public class ExtractProductInfoFromImagesCommand : IRequest<ResponseModel<ExtractProductInfoFromImageResponse>>
    {
        public ExtractProductInfoFromImagesRequest Request;

        public ExtractProductInfoFromImagesCommand(ExtractProductInfoFromImagesRequest request)
        {
            Request = request;
        }

        public class Handler : IRequestHandler<ExtractProductInfoFromImagesCommand, ResponseModel<ExtractProductInfoFromImageResponse>>
        {
            private IGenerativeServices _generativeServices;

            public Handler(IGenerativeServices generativeService)
            {

                _generativeServices = generativeService;
            }

            public async Task<ResponseModel<ExtractProductInfoFromImageResponse>> Handle(ExtractProductInfoFromImagesCommand request, CancellationToken cancellationToken)
            {
                var generativeOptions = new GenerativeContentOptions
                {
                    Prompt = "test"
                };

                // try
                // {
                var result = await _generativeServices.GenerateContentAsync(generativeOptions);
                var res = new ExtractProductInfoFromImageResponse
                {
                    result = result
                };

                return ResponseModel<ExtractProductInfoFromImageResponse>.Success(res, "success");
                // }
                // catch (HttpRequestException ex)
                // {
                //     Conwole.log
                //     throw new NotImplementedException();
                // }
            }
        }
    }
}