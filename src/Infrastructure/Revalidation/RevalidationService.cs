namespace SkorinosGimnazija.Infrastructure.Revalidation;

using System.Net.Http.Json;
using Domain.Options;
using Microsoft.Extensions.Options;

public sealed class RevalidationService : IRevalidationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _token;
    private readonly string _url;

    public RevalidationService(
        IHttpClientFactory httpClientFactory,
        IOptions<PostRevalidationOptions> revalidationOptions)
    {
        _httpClientFactory = httpClientFactory;
        _token = revalidationOptions.Value.Token;
        _url = revalidationOptions.Value.Url;
    }

    public async Task<bool> RevalidateAsync(string language, string slug, int postId)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, _url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", _token },
                { "lang", language },
                { "postId", postId.ToString() },
                { "slug", slug }
            })
        };

        var request = await _httpClientFactory.CreateClient().SendAsync(message);
        var response = await request.Content.ReadFromJsonAsync<RevalidationResponse>();

        return response?.Success == true;
    }

    public async Task<bool> RevalidateAsync(string language)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, _url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", _token },
                { "lang", language }
            })
        };

        var request = await _httpClientFactory.CreateClient().SendAsync(message);
        var response = await request.Content.ReadFromJsonAsync<RevalidationResponse>();

        return response?.Success == true;
    }
}