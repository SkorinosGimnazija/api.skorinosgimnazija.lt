namespace API.Endpoints.Meta.Posts;

using Microsoft.Extensions.Caching.Memory;

public sealed class PostsMetaPublicEndpoint(AppDbContext dbContext, IMemoryCache cache)
    : EndpointWithoutRequest<IEnumerable<LinkMetaResponse>>
{
    public override void Configure()
    {
        Get("public/meta/posts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await cache.GetOrCreateAsync("Meta:Posts", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);

            return await dbContext.Posts.AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.ShowInFeed &&
                           x.PublishedAt <= DateTime.UtcNow &&
                           dbContext.Menus.All(m => m.PostId != x.Id))
                       .Select(x => new LinkMetaResponse
                       {
                           Url = $"/{x.Id}/{x.Slug}",
                           Ln = x.LanguageId,
                           Date = x.ModifiedAt ?? x.PublishedAt
                       })
                       .ToListAsync(ct);
        });

        await Send.OkAsync(entities!, ct);
    }
}