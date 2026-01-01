namespace API.Endpoints.Menus.Create;

using API.Endpoints.Menus.Get;

public sealed class CreateMenuEndpoint(AppDbContext dbContext)
    : Endpoint<CreateMenuRequest, MenuResponse, MenuMapper>
{
    public override void Configure()
    {
        Post("menus");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateMenuRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.Menus.Add(entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        await Send.CreatedAtAsync<GetMenuEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}