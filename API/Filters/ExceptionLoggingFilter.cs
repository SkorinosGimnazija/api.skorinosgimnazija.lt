namespace API.Filters
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger, IWebHostEnvironment env,
            IConfiguration config)
        {
            _logger = logger;
            _env = env;
            _config = config;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if (_env.IsProduction())
            {
                try
                {
                    var message = new ExceptionMessage
                    {
                        Title = context.Exception.GetType().Name,
                        Text = $"<p>{context.Exception}</p>"
                    };

                    var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8,
                        "application/json");

                    using var client = new HttpClient();
                    await client.PostAsync(_config.GetWebHookUrl(), content);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception logging failed");
                }
            }
        }
    }
}