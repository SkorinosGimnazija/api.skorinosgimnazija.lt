namespace API.Services.Captcha;

public record CaptchaResponse
{
    public bool Success { get; init; }

    public double Score { get; init; }
}