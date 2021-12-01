namespace SkorinosGimnazija.Application.Common.Exceptions;

public sealed class SearchIndexException : Exception
{
    public SearchIndexException()
        : base()
    {
    }

    public SearchIndexException(string? message)
        : base(message)
    {
    }

    public SearchIndexException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}