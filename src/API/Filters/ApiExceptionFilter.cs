namespace SkorinosGimnazija.API.Filters;

using System.Reflection;
using Application.Common.Exceptions;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public sealed class ApiExceptionFilter : ExceptionFilterAttribute
{
    private static readonly IDictionary<Type, MethodInfo> ExceptionHandlers =
        typeof(ApiExceptionFilter).GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(x => x.GetCustomAttribute<HandleExceptionAttribute>() is not null)
            .ToDictionary(x => x.GetCustomAttribute<HandleExceptionAttribute>()!.Exception, x => x);

    public override void OnException(ExceptionContext context)
    {
        if (ExceptionHandlers.TryGetValue(context.Exception.GetType(), out var handler))
        {
            handler.Invoke(this, new object[] { context });
        }
    }

    [HandleException(typeof(ValidationException))]
    private static void HandleValidation(ExceptionContext context)
    {
        var exception = (ValidationException) context.Exception;
        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    [HandleException(typeof(NotFoundException))]
    private static void HandleNotFound(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = string.IsNullOrWhiteSpace(exception.Message) ? null : exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    [HandleException(typeof(ImageOptimizationException))]
    private static void HandleImageOptimization(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing images.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }

    [HandleException(typeof(SearchIndexException))]
    private static void HandleSearchIndex(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing search.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.InnerException?.Message ?? exception.Message
        };

        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }

    [HandleException(typeof(UnauthorizedAccessException))]
    private static void HandleUnauthorizedAccess(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Unauthorized",
            Detail = exception.Message
        };

        context.Result = new UnauthorizedObjectResult(details);
        context.ExceptionHandled = true;
    }
}