namespace API.Endpoints.Posts.Public.Get;

public sealed class GetPostByUrlPublicEndpoint(AppDbContext dbContext)
    : Endpoint<GetPostByUrlPublicRequest, GetPostPublicResponse, GetPostPublicMapper>
{
    public override void Configure()
    {
        Get("public/{LanguageId:maxlength(5)}/posts/menu/{MenuUrl:maxlength(256)}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPostByUrlPublicRequest req, CancellationToken ct)
    {
        var url = Uri.UnescapeDataString(req.MenuUrl);
        if (!url.StartsWith('/'))
        {
            url = $"/{url}";
        }

        var entity = await dbContext.Menus.AsNoTracking()
                         .Where(x =>
                             x.LanguageId == req.LanguageId &&
                             x.Url == url &&
                             x.IsPublished &&
                             x.PostId != null)
                         .Select(x => x.Post!)
                         .Where(x => x.IsPublished)
                         .Select(x => Map.FromEntity(x))
                         .FirstOrDefaultAsync(ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(entity, ct);
    }
}