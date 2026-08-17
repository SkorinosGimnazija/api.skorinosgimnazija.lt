namespace API.Services.Captcha;

using API.Services.Options;
using Microsoft.Extensions.Options;

public class CaptchaService(
    IHttpClientFactory httpClientFactory,
    IOptions<CaptchaOptions> captchaOptions)
{
    private const string Url = "https://www.google.com/recaptcha/api/siteverify";

    public async Task<bool> ValidateAsync(string token)
    {
        using var content = new FormUrlEncodedContent([
            // new("remoteip", ip),
            new("secret", captchaOptions.Value.Secret),
            new("response", token)
        ]);

        var client = httpClientFactory.CreateClient();
        using var response = await client.PostAsync(Url, content);
        var data = await response.Content.ReadFromJsonAsync<CaptchaResponse>();

        return data is { Success: true, Score: > 0.5 };
    }
}