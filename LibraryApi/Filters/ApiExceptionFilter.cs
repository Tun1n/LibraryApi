using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryApi.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {

            _logger.LogError(context.Exception, "Unhandled exception: Status Code 500");

            context.Result = new ObjectResult("There was a problem processing your request.: Status Code 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
