namespace SkorinosGimnazija.Infrastructure.Services;

public record CaptchaResponse(bool Success, string Action, float Score);