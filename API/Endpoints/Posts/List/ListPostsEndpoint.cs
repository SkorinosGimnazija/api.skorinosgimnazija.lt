namespace API.Endpoints.Posts.List;

using API.Extensions;

public sealed class ListPostsEndpoint(AppDbContext dbContext)
    : Endpoint<ListPostsRequest, PaginatedListResponse<PostResponse>, PostMapper>
{
    public override void Configure()
    {
        Get("posts");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListPostsRequest req, CancellationToken ct)
    {
        var query = dbContext.Posts.AsNoTracking()
            .Include(x => x.Language)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.SearchTerm))
        {
            var search = req.SearchTerm.NormalizeLithuanian();
            query = query.Where(x =>
                    EF.Functions.TrigramsStrictWordSimilarity(x.NormalizedTitle, search) > 0.1)
                .OrderByDescending(x =>
                    EF.Functions.TrigramsStrictWordSimilarity(x.NormalizedTitle, search));
        }
        else
        {
            query = query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.PublishedAt);
        }

        var entities = await query
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }
}