using API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                var errorDetails = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(errorDetails, exception.StackTrace);                          
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Messenger = "Internal Server Error! Have A Mistake on Server Side",
                    ErrorsDetail = errorDetails.Split(new[] { "\r\n" }, StringSplitOptions.None)
                }.ToString());
            }          
        }
    }
}
