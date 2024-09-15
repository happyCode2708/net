using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
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
            public Task<ResponseModel<ExtractProductInfoFromImageResponse>> Handle(ExtractProductInfoFromImagesCommand request, CancellationToken cancellationToken)
            {

                throw new NotImplementedException();
            }



        }
    }
}