namespace SkorinosGimnazija.Domain.Options;

public record JwtOptions
{
    public string Secret { get; init; } = default!;

    public string Audience { get; init; } = default!;

    public string Issuer { get; init; } = default!;
}