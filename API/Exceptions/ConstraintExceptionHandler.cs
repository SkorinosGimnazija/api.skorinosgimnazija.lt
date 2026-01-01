namespace API.Exceptions;

using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;
using Npgsql;

internal sealed class ConstraintExceptionHandler(IProblemDetailsService problemDetails)
    : IExceptionHandler
{
    private const int ExceptionStatusCode = StatusCodes.Status409Conflict;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not (UniqueConstraintException or ReferenceConstraintException))
        {
            return false;
        }

        httpContext.Response.StatusCode = ExceptionStatusCode;

        return await problemDetails.TryWriteAsync(new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = ExceptionStatusCode,
                Title = exception.Message,
                Detail = (exception.InnerException as PostgresException)?.ConstraintName
            }
        });
    }
}