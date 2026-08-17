namespace API.Endpoints.Events;

using API.Services.Revalidation;

public sealed class EventRevalidation<TRequest, TResponse>(RevalidationService revalidationService)
    : IPostProcessor<TRequest, TResponse>
{
    private const string Tag = "events";

    public async Task PostProcessAsync(IPostProcessorContext<TRequest, TResponse> ctx, CancellationToken ct)
    {
        await revalidationService.RevalidateAsync(new() { Tag = Tag }, ct);
    }
}