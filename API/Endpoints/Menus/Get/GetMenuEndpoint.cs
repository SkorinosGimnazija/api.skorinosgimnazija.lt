namespace API.Endpoints.Menus.Get;

public sealed class GetMenuEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, MenuResponse, MenuMapper>
{
    public override void Configure()
    {
        Get("menus/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Menus.AsNoTracking()
                         .Include(x => x.Language)
                         .Include(x => x.Post)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}