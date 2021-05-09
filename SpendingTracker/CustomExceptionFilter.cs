using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SpendingTracker.Models.Error;

namespace SpendingTracker
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case HttpStatusException httpStatusException:
                    context.Result = new StatusCodeResult(httpStatusException.StatusCode);
                    return;
                case ValidationException validationException:
                    context.Result = new BadRequestObjectResult(new ValidationErrorResponse
                    {
                        ErrorMessages = validationException.ErrorMessages
                    });
                    return;
                default: throw context.Exception;
            }
        }
    }
}