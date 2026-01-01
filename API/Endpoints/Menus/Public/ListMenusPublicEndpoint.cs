namespace API.Endpoints.Menus.Public;

using API.Database.Entities.CMS;

public sealed class ListMenusPublicEndpoint(AppDbContext dbContext)
    : Endpoint<ListMenusPublicRequest, IEnumerable<ListMenusPublicResponse>, ListMenusPublicMapper>
{
    public override void Configure()
    {
        Get("public/{languageId:maxlength(5)}/menus");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListMenusPublicRequest req, CancellationToken ct)
    {
        var entities = await dbContext.Menus.AsNoTracking()
                           .Where(x => x.LanguageId == req.LanguageId &&
                                       x.IsPublished &&
                                       !x.IsHidden)
                           .OrderBy(x => x.Order)
                           .ThenBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(GetMenuTree(entities), ct);
    }

    public List<ListMenusPublicResponse> GetMenuTree(List<Menu> allMenus)
    {
        var lookup = allMenus.ToLookup(x => x.ParentMenuId);
        return Build(null);

        List<ListMenusPublicResponse> Build(int? parentId)
        {
            return lookup[parentId]
                .Select(x => Map.FromEntity(x, Build(x.Id)))
                .ToList();
        }
    }
}