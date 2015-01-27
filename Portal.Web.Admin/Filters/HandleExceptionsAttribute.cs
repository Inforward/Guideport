using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Portal.Infrastructure.Logging;

namespace Portal.Web.Admin.Filters
{
    public class HandleExceptionsAttribute : FilterAttribute
    {
        
    }

    public class HandleExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public HandleExceptionsFilter(ILogger logger)
        {
            _logger = logger;
        }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            var context = HttpContext.Current;

            return Task.Factory.StartNew(() => _logger.Log(context, actionExecutedContext.Exception), cancellationToken);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}