using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Enums.Logging;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILoggingService _logger;
        public ErrorController(ILoggingService logger)
        {
            _logger = logger;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/api/error")]
        public async Task<IActionResult> Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception ex = context.Error;

            //handle internal server errors

            await _logger.Log(context.Error.Message, LogLevel.Critical);

            return Problem();
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/api/error-local-development")]
        public async Task<IActionResult> ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                string msg = "This shouldn't be invoked in non-development environments.";
                await _logger.Log(msg, LogLevel.Critical);
                throw new InvalidOperationException(msg);
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            await _logger.Log(context.Error.Message, LogLevel.Critical);

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}