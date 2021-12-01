namespace SkorinosGimnazija.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException()
        : this(string.Empty)
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}