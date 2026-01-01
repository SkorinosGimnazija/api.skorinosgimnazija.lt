namespace API.Endpoints.Menus.List;

using API.Database.Entities.CMS;

public sealed class ListMenusEndpoint(AppDbContext dbContext)
    : Endpoint<ListMenusRequest, IEnumerable<MenuResponse>, MenuMapper>
{
    public override void Configure()
    {
        Get("menus");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListMenusRequest req, CancellationToken ct)
    {
        var query = dbContext.Menus.AsNoTracking()
            .Include(x => x.Language)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.LanguageId))
        {
            query = query.Where(x => x.LanguageId == req.LanguageId);
        }

        var entities = await query
                           .OrderBy(x => x.Order)
                           .ThenBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(GetMenuTree(entities), ct);
    }

    public List<MenuResponse> GetMenuTree(List<Menu> allMenus)
    {
        var lookup = allMenus.ToLookup(x => x.ParentMenuId);
        return Build(null);

        List<MenuResponse> Build(int? parentId)
        {
            return lookup[parentId]
                .Select(x => Map.FromEntity(x, Build(x.Id)))
                .ToList();
        }
    }
}