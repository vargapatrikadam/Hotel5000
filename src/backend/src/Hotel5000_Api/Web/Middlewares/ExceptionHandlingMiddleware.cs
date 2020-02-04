using Core.Enums.Logging;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.DTOs;

namespace Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILoggingService _loggingService;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //Get innermost exception
            while (ex.InnerException != null) ex = ex.InnerException;

            // Specify different custom exceptions here
            if (ex is ArgumentException || ex is SqlException) code = HttpStatusCode.BadRequest;

            var response = new ErrorDto();
            response.Message = (int) code == 500 ? "Internal server error" : ex.Message;
            //string result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            var level = LogLevel.Warning;
            if ((int) code == 500) level = LogLevel.Critical;
            await _loggingService.Log(ex.Message, level);

            await context.Response.WriteAsync(response.ToString());
        }
    }
}