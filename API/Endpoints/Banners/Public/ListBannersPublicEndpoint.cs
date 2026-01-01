namespace API.Endpoints.Banners.Public;

public sealed class ListBannersPublicEndpoint(AppDbContext dbContext)
    : Endpoint<ListBannersPublicRequest, IEnumerable<ListBannersPublicResponse>,
        ListBannersPublicMapper>
{
    public override void Configure()
    {
        Get("public/{languageId:maxlength(5)}/featured");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListBannersPublicRequest req, CancellationToken ct)
    {
        var entities = await dbContext.Banners.AsNoTracking()
                           .Where(x => x.LanguageId == req.LanguageId && x.IsPublished)
                           .OrderBy(x => x.Order)
                           .ThenBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}