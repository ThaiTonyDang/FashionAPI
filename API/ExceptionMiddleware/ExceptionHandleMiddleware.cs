using API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
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
                _logger.LogError(errorDetails, exception.StackTrace);
                await HandleExceptionAsync(context, exception, errorDetails);
            }           
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string errorDetails)
        {                   
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new Error()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error ! There is an error on the server side",
                Errors = errorDetails.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)
            }.ToString()); ;
        }
    }
}
