using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace UrlShortening.API.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public CustomExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// On Exception Occured in API action.
        /// </summary>
        /// <param name="context">context.</param>
        public override void OnException(ExceptionContext context)
        {
            this._logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
