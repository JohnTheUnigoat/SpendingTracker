using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpendingTracker
{
    public class HttpStatusExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpStatusException httpStatusException)
            {
                context.Result = new StatusCodeResult(httpStatusException.StatusCode);
                return;
            }

            throw context.Exception;
        }
    }
}