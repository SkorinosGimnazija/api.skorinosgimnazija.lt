namespace SkorinosGimnazija.Application.Common.Exceptions;

public sealed class ImageOptimizationException : Exception
{
    public ImageOptimizationException()
        : base()
    {
    }

    public ImageOptimizationException(string? message)
        : base(message)
    {
    }

    public ImageOptimizationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}