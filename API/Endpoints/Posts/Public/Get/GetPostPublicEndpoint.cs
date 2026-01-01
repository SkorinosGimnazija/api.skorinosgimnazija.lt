namespace API.Endpoints.Posts.Public.Get;

public sealed class GetPostPublicEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, GetPostPublicResponse, GetPostPublicMapper>
{
    public override void Configure()
    {
        Get("public/posts/{id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Posts.AsNoTracking()
                         .Where(x =>
                             x.IsPublished &&
                             x.PublishedAt <= DateTime.UtcNow &&
                             x.Id == req.Id)
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