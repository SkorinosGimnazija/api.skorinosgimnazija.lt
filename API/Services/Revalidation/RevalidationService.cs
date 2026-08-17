namespace API.Services.Revalidation;

using API.Services.Options;
using Microsoft.Extensions.Options;

public class RevalidationService(
    ILogger<RevalidationService> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<PostRevalidationOptions> revalidationOptions)
{
    public async Task<bool> RevalidateAsync(RevalidationRequest request, CancellationToken ct)
    {
        try
        {
            var client = httpClientFactory.CreateClient(nameof(RevalidationService));
            using var response = await client.PostAsJsonAsync(revalidationOptions.Value.Url, request, ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Revalidation failed ({code})", response.StatusCode);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Revalidation failed");
        }

        return false;
    }
}