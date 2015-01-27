using System;
using System.Web.Mvc;
using Portal.Infrastructure.Logging;

namespace Portal.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HandleExceptionsAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }

    public class HandleExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public HandleExceptionsFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            _logger.Log(null, filterContext.Exception);

            filterContext.Result = new RedirectToRouteResult("Error", null);
        }
    }
}