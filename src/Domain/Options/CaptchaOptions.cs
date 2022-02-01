namespace SkorinosGimnazija.Infrastructure.Options;

public record CaptchaOptions
{
    public string Secret { get; set; } = default!;
}