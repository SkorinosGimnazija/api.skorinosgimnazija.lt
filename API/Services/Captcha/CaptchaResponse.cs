namespace API.Services.Captcha;

public record CaptchaResponse
{
    public required bool Success { get; init; }

    public required string Action { get; init; }

    public required double Score { get; init; }
}