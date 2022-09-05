using Esoteric.Finance.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Esoteric.Finance.Api.Handlers
{
    public class HttpStatusCodeExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpStatusCodeException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = (int) httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
            else if (context.Exception is not null)
            {
                context.Result = new ObjectResult(new { context.Exception.Message, context.Exception.StackTrace })
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
