namespace API.Endpoints.Posts.Public.List;

using API.Extensions;

public sealed class ListPostsPublicEndpoint(AppDbContext dbContext)
    : Endpoint<ListPostsPublicRequest, PaginatedListResponse<ListPostPublicResponse>,
        ListPostPublicMapper>
{
    public override void Configure()
    {
        Get("public/{languageId:maxlength(5)}/posts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListPostsPublicRequest req, CancellationToken ct)
    {
        var entities = await dbContext.Posts.AsNoTracking()
                           .Where(x =>
                               x.IsPublished &&
                               x.ShowInFeed &&
                               x.PublishedAt <= DateTime.UtcNow &&
                               x.LanguageId == req.LanguageId)
                           .OrderByDescending(x => x.IsFeatured)
                           .ThenByDescending(x => x.PublishedAt)
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }
}