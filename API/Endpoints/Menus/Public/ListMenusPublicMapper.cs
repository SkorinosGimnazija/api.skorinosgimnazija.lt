namespace API.Endpoints.Menus.Public;

using API.Database.Entities.CMS;

public sealed class ListMenusPublicMapper : ResponseMapper<ListMenusPublicResponse, Menu>
{
    public ListMenusPublicResponse FromEntity(Menu e, List<ListMenusPublicResponse> children)
    {
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Url = e.Url,
            IsExternal = e.IsExternal,
            Children = children.Count > 0 ? children : null
        };
    }
}