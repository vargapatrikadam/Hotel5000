using Core.Helpers.Results;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;

namespace Web.Helpers
{
    public static class ControllerExtensions
    {
        public static IActionResult GetError<T>(this ControllerBase controller, Result<T> result) 
            => result.ResultType switch
        {
            ResultType.Invalid      => controller.BadRequest(new ErrorDto(result.Errors)),
            ResultType.NotFound     => controller.NotFound(new ErrorDto(result.Errors)),
            ResultType.Unauthorized => controller.Unauthorized(new ErrorDto(result.Errors)),
            ResultType.Unexpected   => controller.BadRequest(new ErrorDto(result.Errors)),
            ResultType.Conflict     => controller.Conflict(new ErrorDto(result.Errors)),
            _                       => controller.BadRequest(new ErrorDto(result.Errors))
        };
    }
}
