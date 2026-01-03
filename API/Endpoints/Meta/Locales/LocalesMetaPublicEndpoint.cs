namespace API.Endpoints.Meta.Locales;

using Microsoft.Extensions.Caching.Memory;

public sealed class LocalesMetaPublicEndpoint(AppDbContext dbContext, IMemoryCache cache)
    : EndpointWithoutRequest<IEnumerable<LinkMetaResponse>>
{
    public override void Configure()
    {
        Get("public/meta/locales");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await cache.GetOrCreateAsync("Meta:Locales", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);

            return await dbContext.Languages.AsNoTracking()
                       .Select(x => new LinkMetaResponse
                       {
                           Url = x.Id,
                           Ln = x.Id,
                           Date = dbContext.Posts
                                      .Where(p =>
                                          p.LanguageId == x.Id &&
                                          p.IsPublished &&
                                          p.ShowInFeed)
                                      .Select(p => (DateTime?) (p.ModifiedAt ?? p.PublishedAt))
                                      .Max() ?? DateTime.MinValue
                       })
                       .ToListAsync(ct);
        });

        await Send.OkAsync(entities!, ct);
    }
}