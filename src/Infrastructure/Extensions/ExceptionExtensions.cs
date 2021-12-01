namespace SkorinosGimnazija.Infrastructure.Extensions;

using Npgsql;

internal static class ExceptionExtensions
{
    public static bool IsConstraintViolation(this PostgresException exception)
    {
        return exception.SqlState is
                   PostgresErrorCodes.IntegrityConstraintViolation or
                   PostgresErrorCodes.RestrictViolation or
                   PostgresErrorCodes.NotNullViolation or
                   PostgresErrorCodes.ForeignKeyViolation or
                   PostgresErrorCodes.UniqueViolation or
                   PostgresErrorCodes.CheckViolation or
                   PostgresErrorCodes.ExclusionViolation;
    }
}