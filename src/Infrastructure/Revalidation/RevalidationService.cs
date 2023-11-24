namespace SkorinosGimnazija.Infrastructure.Revalidation;

using System.Net.Http.Json;
using Application.Common.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed class RevalidationService : IRevalidationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RevalidationService> _logger;
    private readonly string _token;
    private readonly string _url;

    public RevalidationService(
        IHttpClientFactory httpClientFactory,
        IOptions<PostRevalidationOptions> revalidationOptions,
        ILogger<RevalidationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _token = revalidationOptions.Value.Token;
        _url = revalidationOptions.Value.Url;
    }

    public async Task<bool> RevalidateAsync(string language, string slug, int postId)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(0, e, "Revalidation failed");
        }

        return false;
    }

    public async Task<bool> RevalidateAsync(string language)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(0, e, "Revalidation failed");
        }

        return false;
    }
}