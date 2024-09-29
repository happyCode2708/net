using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.ObjectPool;
using MyApi.application.common.enums;
using MyApi.application.common.interfaces;
using MyApi.core.controllers;
using Newtonsoft.Json.Linq;

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
                    Prompt = "What is capital of USA?",
                    ModelId = GenerativeModelEnum.Gemini_1_5_Flash_001,
                };

                // try
                // {
                var result = await _generativeServices.GenerateContentAsync(generativeOptions);

                //* get concat text results

                JArray resultArray = JArray.Parse(result);

                var ConcatResult = String.Join(" ", resultArray.Select(r => r["candidates"]?.First?["content"]?["parts"]?.First?["text"]));

                var res = new ExtractProductInfoFromImageResponse
                {
                    FullResult = result,
                    ConcatText = ConcatResult,
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