namespace API.Endpoints.Posts.Get;

public sealed class GetPostEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, PostResponse, PostMapper>
{
    public override void Configure()
    {
        Get("posts/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Posts.AsNoTracking()
                         .Include(x => x.Language)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}