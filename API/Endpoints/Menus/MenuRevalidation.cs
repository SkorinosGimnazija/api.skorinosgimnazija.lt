namespace API.Endpoints.Menus;

using API.Services.Revalidation;

public sealed class MenuRevalidation<TRequest, TResponse>(RevalidationService revalidationService)
    : IPostProcessor<TRequest, TResponse>
{
    private const string Tag = "menus";

    public async Task PostProcessAsync(IPostProcessorContext<TRequest, TResponse> ctx, CancellationToken ct)
    {
        await revalidationService.RevalidateAsync(new() { Tag = Tag }, ct);
    }
}