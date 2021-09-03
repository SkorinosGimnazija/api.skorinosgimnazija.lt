namespace API.Filters
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;

    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (_env.IsProduction())
            {
                // todo log to db
                //_logger.LogError(context.Exception, context.Exception.Message);
            }

            return base.OnExceptionAsync(context);
        }
    }
}