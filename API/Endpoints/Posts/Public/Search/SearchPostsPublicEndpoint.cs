namespace API.Endpoints.Posts.Public.Search;

using API.Endpoints.Posts.Public.List;
using API.Extensions;

public sealed class SearchPostsPublicEndpoint(AppDbContext dbContext)
    : Endpoint<SearchPostsPublicRequest, PaginatedListResponse<ListPostPublicResponse>, ListPostPublicMapper>
{
    public override void Configure()
    {
        Get("public/{languageId:maxlength(5)}/posts/search");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchPostsPublicRequest req, CancellationToken ct)
    {
        var term = req.SearchTerm.Trim();
        var normalizedTerm = term.NormalizeLithuanian();

        var entities = await dbContext.Posts.AsNoTracking()
           .Where(x => x.IsPublished && x.LanguageId == req.LanguageId)
           .Where(x =>
               EF.Functions.TrigramsStrictWordSimilarity(normalizedTerm, x.NormalizedTitle) > 0.5 ||
               EF.Functions.TrigramsStrictWordSimilarity(term, x.Text) > 0.5)
           // .OrderByDescending(x => EF.Functions.TrigramsStrictWordSimilarity(normalizedTerm, x.NormalizedTitle))
           // .ThenByDescending(x => EF.Functions.TrigramsStrictWordSimilarity(term, x.Text))
           .OrderByDescending(x => Math.Max(
               EF.Functions.TrigramsStrictWordSimilarity(normalizedTerm, x.NormalizedTitle),
               EF.Functions.TrigramsStrictWordSimilarity(term, x.Text)))
           .ThenByDescending(x => x.Id)
           .Select(x => Map.FromEntity(x))
           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }
}