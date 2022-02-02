namespace SkorinosGimnazija.Domain.Options;

public record EmailOptions
{
    public string SenderName { get; init; } = default!;

    public string SenderEmail { get; init; } = default!;
}