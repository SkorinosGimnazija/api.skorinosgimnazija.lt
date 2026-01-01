namespace API.Exceptions;

using EntityFramework.Exceptions.Common;
using Sentry.Extensibility;

public sealed class SentryIgnoredExceptions : IExceptionFilter
{
    private static readonly HashSet<Type> IgnoredExceptionTypes =
    [
        typeof(OperationCanceledException),
        typeof(UniqueConstraintException),
        typeof(ReferenceConstraintException)
    ];

    public bool Filter(Exception ex)
    {
        return IgnoredExceptionTypes.Contains(ex.GetType());
    }
}