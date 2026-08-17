namespace API.Endpoints.Posts;

using API.Services.Revalidation;

public sealed class PostRevalidation<TRequest, TResponse>(RevalidationService revalidationService)
    : IPostProcessor<TRequest, TResponse>
{
    private const string Tag = "posts";

    public async Task PostProcessAsync(IPostProcessorContext<TRequest, TResponse> ctx, CancellationToken ct)
    {
        if (ctx.Response is PostResponse response)
        {
            var db = ctx.HttpContext.Resolve<AppDbContext>();
            var menuUrl = await db.Menus.AsNoTracking()
                              .Where(x => x.PostId == response.Id)
                              .Select(x => x.Url)
                              .FirstOrDefaultAsync(ct);

            await revalidationService.RevalidateAsync(new()
            {
                Tag = Tag,
                Id = response.Id,
                Slug = menuUrl
            }, ct);
        }
        else if (ctx.Request is RouteIdRequest request)
        {
            await revalidationService.RevalidateAsync(new() { Tag = Tag, Id = request.Id }, ct);
        }
    }
}