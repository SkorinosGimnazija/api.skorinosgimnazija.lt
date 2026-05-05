namespace API.Exceptions;

using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;

internal sealed class ConstraintExceptionHandler(IProblemDetailsService problemDetails)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not (UniqueConstraintException or ReferenceConstraintException))
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        var errorDetails = new
        {
            Name = exception switch
            {
                UniqueConstraintException e => e.ConstraintName,
                ReferenceConstraintException e => e.ConstraintName,
                _ => null
            },
            Reason = exception.Data[nameof(exception.Message)] ?? exception.Message
        };

        return await problemDetails.TryWriteAsync(new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = httpContext.Response.StatusCode,
                Detail = exception.Message,
                Extensions = { ["errors"] = new[] { errorDetails } }
            }
        });
    }
}