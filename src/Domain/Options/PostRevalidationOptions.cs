namespace SkorinosGimnazija.Domain.Options;

public record PostRevalidationOptions
{
    public string Token { get; set; } = default!;

    public string Url { get; set; } = default!;
}