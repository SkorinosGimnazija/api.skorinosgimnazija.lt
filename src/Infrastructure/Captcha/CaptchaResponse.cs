namespace SkorinosGimnazija.Infrastructure.Captcha;

public record CaptchaResponse(bool Success, string Action, float Score);