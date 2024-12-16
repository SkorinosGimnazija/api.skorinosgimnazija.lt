namespace SkorinosGimnazija.Domain.Options;

public record GroupOptions
{
    public string Service { get; init; } = default!;

    public string Managers { get; init; } = default!;

    public string Teachers { get; init; } = default!;

    public string SocialManagers { get; init; } = default!;

    public string TechManagers { get; init; } = default!;

    public string TechNotifications { get; init; } = default!;

    public string SocialNotifications { get; init; } = default!;

    public string TechStatusNotifications { get; init; } = default!;
}