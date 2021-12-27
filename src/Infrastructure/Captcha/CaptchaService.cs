namespace SkorinosGimnazija.Infrastructure.Services;

using System.Net.Http.Json;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Options;

public class CaptchaService : ICaptchaService
{
    private const string BaseUrl = "https://www.google.com/recaptcha/api/siteverify";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _secret;

    public CaptchaService(
        IHttpClientFactory httpClientFactory,
        IOptions<CaptchaOptions> captchaOptions)
    {
        _httpClientFactory = httpClientFactory;
        _secret = captchaOptions.Value.Secret;
    }

    public async Task<bool> ValidateAsync(string token)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                //{ "remoteip", ip},
                { "secret", _secret },
                { "response", token }
            })
        };

        var request = await _httpClientFactory.CreateClient().SendAsync(message);
        var response = await request.Content.ReadFromJsonAsync<CaptchaResponse>();

        return response?.Success == true && response.Score > 0;
    }
}