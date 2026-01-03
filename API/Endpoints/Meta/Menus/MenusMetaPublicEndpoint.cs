namespace API.Endpoints.Meta.Menus;

using Microsoft.Extensions.Caching.Memory;

public sealed class MenusMetaPublicEndpoint(AppDbContext dbContext, IMemoryCache cache)
    : EndpointWithoutRequest<IEnumerable<LinkMetaResponse>>
{
    public override void Configure()
    {
        Get("public/meta/menus");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await cache.GetOrCreateAsync("Meta:Menus", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);

            return await dbContext.Menus.AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           !x.IsHidden &&
                           x.Url != null &&
                           x.PostId != null)
                       .Select(x => new LinkMetaResponse
                       {
                           Url = x.Url!,
                           Ln = x.LanguageId,
                           Date = x.Post!.ModifiedAt ?? x.Post!.PublishedAt
                       })
                       .ToListAsync(ct);
        });

        await Send.OkAsync(entities!, ct);
    }
}