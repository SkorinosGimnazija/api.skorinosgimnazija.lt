namespace API.Endpoints.Menus.Update;

public sealed class UpdateMenuEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateMenuRequest, MenuResponse, MenuMapper>
{
    public override void Configure()
    {
        Put("menus");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateMenuRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Menus.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);

        if (entity.Url is not null)
        {
            var hasChildren = await dbContext.Menus.AnyAsync(x => x.ParentMenuId == req.Id, ct);
            if (hasChildren)
            {
                ThrowError(x => x.Url, "Menu with submenus cannot have a URL");
            }
        }

        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}