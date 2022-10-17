namespace SkorinosGimnazija.Domain.Options;

public record GroupOptions
{
    public string Service { get; init; } = default!;

    public string Managers { get; init; } = default!;

    public string Teachers { get; init; } = default!;

    public string BullyManagers { get; init; } = default!;

    public string TechManagers { get; init; } = default!;
}