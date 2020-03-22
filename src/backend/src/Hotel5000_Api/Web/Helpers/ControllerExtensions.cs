using Core.Helpers.Results;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;

namespace Web.Helpers
{
    public static class ControllerExtensions
    {
        public static IActionResult GetError<T>(this ControllerBase controller, Result<T> result)
        {
            switch (result.ResultType)
            {
                case ResultType.Invalid: return controller.BadRequest(new ErrorDto(result.Errors));
                case ResultType.NotFound: return controller.NotFound(new ErrorDto(result.Errors));
                case ResultType.Unauthorized: return controller.Unauthorized(new ErrorDto(result.Errors));
                case ResultType.Unexpected: return controller.BadRequest(new ErrorDto(result.Errors));
                case ResultType.Conflict: return controller.Conflict(new ErrorDto(result.Errors));
                default: return controller.BadRequest(new ErrorDto(result.Errors));
            }
        }
    }
}
