namespace SkorinosGimnazija.Domain.Options;

public record CaptchaOptions
{
    public string Secret { get; set; } = default!;
}