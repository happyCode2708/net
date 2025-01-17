using System;
using System.ComponentModel;

namespace MyApi.Core.Controllers
{
    public class ResponseModel
    {
        [Description("Does API has been called successfully")]
        public required bool IsSuccess { get; set; }
        [Description("Message returns from API")]
        public string? Message { get; set; }
        [Description("Exception returns from API")]
        public string? Exception { get; set; }
        public static ResponseModel Fail(string? message = null, Exception? exception = null)
        {
            return new ResponseModel { IsSuccess = false, Exception = exception?.ToString(), Message = message ?? exception?.Message };
        }

        public static ResponseModel Success(string? message = null)
        {
            return new ResponseModel { IsSuccess = true, Message = message };
        }
    }
    public class ResponseModel<T> : ResponseModel
    {
        public T? Data { get; set; }

        public ResponseModel(T? data)
        {
            Data = data;
        }

        public static ResponseModel<T> Fail(string? message = null, string? exception = null, T? data = default)
        {
            return new ResponseModel<T>(data) { IsSuccess = false, Exception = exception, Message = message ?? exception };
        }

        public static ResponseModel<T> Success(T data, string? message = null)
        {
            var result = new ResponseModel<T>(data) { IsSuccess = true, Message = message };

            return result;
        }
    }
}