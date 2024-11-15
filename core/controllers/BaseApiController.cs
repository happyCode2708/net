using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Core.Controllers
{
    /// <summary>
    /// the base class for all APIs, apply mediator pattern
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// common Mediator object
        /// </summary>
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        /// <summary>
        /// Base method for all commands
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        protected async Task<TResult> CommandAsync<TResult>(IRequest<TResult> command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// base method for all queries
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected async Task<TResult> QueryAsync<TResult>(IRequest<TResult> query)
        {
            // if (_mediator == null)
            // {
            //     throw new InvalidOperationException("Mediator is not initialized.");
            // }

            return await Mediator.Send(query);
        }

        /// <summary>
        /// base method for all events
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected async Task PublishAsync<TResult>(IRequest<TResult> query)
        {
            await Mediator.Publish(query);
        }

        /// <summary>
        /// base method for all events
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        protected async Task PublishAsync(INotification notification)
        {
            await Mediator.Publish(notification);
        }

        protected Task SendAsync<TRequest>(TRequest request) where TRequest : IRequest
        {
            return Mediator.Send(request);
        }

        protected OkObjectResult ReturnObject<T>(T data, bool isSuccess = true, string message = null, string exception = null)
        {
            var responseModel = new ResponseModel<T>(data) { IsSuccess = isSuccess, Message = message, Exception = exception };
            return Ok(responseModel);
        }

        protected OkObjectResult ReturnSuccess(string message = null, string exception = null)
        {
            var responseModel = new ResponseModel { IsSuccess = true, Message = message, Exception = exception };
            return Ok(responseModel);
        }
        protected OkObjectResult ReturnFailure(string message = null, string exception = null)
        {
            var responseModel = new ResponseModel { IsSuccess = false, Message = message, Exception = exception };
            return Ok(responseModel);
        }
    }

}
