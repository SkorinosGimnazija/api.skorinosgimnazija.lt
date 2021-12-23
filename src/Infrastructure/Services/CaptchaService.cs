namespace SkorinosGimnazija.Infrastructure.Services;

using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Options;

public class CaptchaService
{
    private const string BaseUrl = "https://www.google.com/recaptcha/api/siteverify";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _secret;

    public CaptchaService(IHttpClientFactory httpClientFactory, IOptions<CaptchaOptions> captchaOptions)
    {
        _httpClientFactory = httpClientFactory;
        _secret = captchaOptions.Value.Secret;
    }

    public async Task<bool> Validate(string token)
    {
        //var req = new HttpRequestMessage(HttpMethod.Get, BaseUrl)
        //{
        //    Content = new FormUrlEncodedContent(new Dictionary<string, string>
        //    {
        //        { "secret", _secret },
        //        { "response", token }
        //    })
        //};

        //var resp = await _httpClientFactory.CreateClient().SendAsync(req);
        //var obj = await resp.Content.ReadFromJsonAsync<CaptchaResponse>();

        var httpClient = _httpClientFactory.CreateClient();
        var url = $"{BaseUrl}?secret={_secret}&response={token}";
        var response = await httpClient.GetFromJsonAsync<CaptchaResponse>(new Uri(url));

        return response?.Success == true;
    }

    public record CaptchaResponse(bool Success, string Action, float Score);
}