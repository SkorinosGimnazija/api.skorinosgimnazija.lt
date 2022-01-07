namespace SkorinosGimnazija.Application.Common.Exceptions;

using FluentValidation.Results;

public sealed class ValidationException : Exception
{
    public ValidationException(string property, string message)
        : this(new ValidationFailure(property, message))
    {
    }

    public ValidationException(ValidationFailure failure)
        : this(new[] { failure })
    {
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures.GroupBy(x => x.PropertyName?.Split('.')[^1])
            .ToDictionary(x => x.Key ?? string.Empty, x => x.Select(z => z.ErrorMessage).ToArray());
    }

    private ValidationException(string message)
        : base(message)
    {
    }

    private ValidationException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    private ValidationException()
        : base()
    {
    }

    public IDictionary<string, string[]> Errors { get; } = default!;
}