namespace API.Filters;

using Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

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
        if (!_env.IsProduction() || context.Exception is OperationCanceledException)
        {
            return;
        }

        try
        {
            var message = new ExceptionMessage
            {
                Title = context.Exception.GetType().Name, Text = $"<p>{context.Exception}</p>"
            };

            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using var client = new HttpClient();
            await client.PostAsync(_config.GetWebHookUrl(), content);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception logging failed");
        }
    }
}