namespace API.Exceptions;

using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;
using Npgsql;

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

        if (exception.InnerException is not PostgresException pgException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        var errorDetails = new
        {
            Name = pgException.ConstraintName,
            Reason = exception.Data[nameof(pgException.MessageText)] ?? pgException.MessageText
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