using Core.Enums.Logging;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private ILoggingService loggingService;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILoggingService LoggingService)
        {
            loggingService = LoggingService;
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //Get innermost exception
            while (ex.InnerException != null) ex = ex.InnerException;

            // Specify different custom exceptions here
            if (ex is ArgumentException) code = HttpStatusCode.BadRequest;

            string result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            LogLevel level = LogLevel.Warning;
            if ((int)code == 500) level = LogLevel.Critical;
            await loggingService.Log(ex.Message, level);

            //TODO: if internal server error, dont write the exception message

            await context.Response.WriteAsync(result);
        }
    }
}
