using API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.ExceptionMiddleware
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(ILogger<ExceptionHandleMiddleware> logger, RequestDelegate next)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var errorMessages = exception.Message;
                _logger.LogError(errorMessages, exception.StackTrace);
                await HandleExceptionAsync(context, errorMessages);
            }           
        }

        private static async Task HandleExceptionAsync(HttpContext context, string messages)
        {                   
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = new Error(
                    context.Response.StatusCode,
                    "Internal Server Error ! There is an error on the server side",
                    messages.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)
                );

            await context.Response.WriteAsync(error.ToString());
        }
    }
}
