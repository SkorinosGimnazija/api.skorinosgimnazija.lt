namespace API.Endpoints.Banners;

using API.Services.Revalidation;

public sealed class BannerRevalidation<TRequest, TResponse>(RevalidationService revalidationService)
    : IPostProcessor<TRequest, TResponse>
{
    private const string Tag = "banners";

    public async Task PostProcessAsync(IPostProcessorContext<TRequest, TResponse> ctx, CancellationToken ct)
    {
        await revalidationService.RevalidateAsync(new() { Tag = Tag }, ct);
    }
}