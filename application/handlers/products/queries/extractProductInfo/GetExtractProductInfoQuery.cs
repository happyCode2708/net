using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MyApi.core.controllers;

namespace MyApi.application.handlers.products.queries.extractProductInfo
{



    public class GetExtractProductInfoQuery : IRequest<ResponseModel<GetExtractProductInfoResponse>>
    {


        public int ProductId { get; set; }

        public GetExtractProductInfoQuery(int productId)
        {
            ProductId = productId;
        }

        public class Handler : IRequestHandler<GetExtractProductInfoQuery, ResponseModel<GetExtractProductInfoResponse>>
        {
            public Task<ResponseModel<GetExtractProductInfoResponse>> Handle(GetExtractProductInfoQuery request, CancellationToken cancellationToken)
            {
                // throw new NotImplementedException();
                // var object final = new { result: 'test' }
                var result = new GetExtractProductInfoResponse()
                {
                    SessionId = "10",
                    Status = "SUCCESS",
                    Result = { },
                };

                return Task.FromResult(ResponseModel<GetExtractProductInfoResponse>.Success(result));
            }

        }
    }
}