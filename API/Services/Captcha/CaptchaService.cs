namespace API.Services.Captcha;

using API.Services.Options;
using Microsoft.Extensions.Options;

public class CaptchaService(
    IHttpClientFactory httpClientFactory,
    IOptions<CaptchaOptions> captchaOptions)
{
    private const string BaseUrl = "https://www.google.com/recaptcha/api/siteverify";

    public async Task<bool> ValidateAsync(string token)
    {
        using var client = httpClientFactory.CreateClient();

        using var content = new FormUrlEncodedContent([
            // new("remoteip", ip),
            new("secret", captchaOptions.Value.Secret),
            new("response", token)
        ]);

        var request = await client.PostAsync(BaseUrl, content);
        var response = await request.Content.ReadFromJsonAsync<CaptchaResponse>();

        return response is { Success: true, Score: > 0.5 };
    }
}