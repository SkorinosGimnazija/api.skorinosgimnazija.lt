namespace API.Endpoints.Menus;

using API.Database.Entities.CMS;
using API.Endpoints.Menus.Create;

public sealed class MenuMapper : Mapper<CreateMenuRequest, MenuResponse, Menu>
{
    public override Menu ToEntity(CreateMenuRequest r)
    {
        return new()
        {
            Order = r.Order,
            IsPublished = r.IsPublished,
            IsHidden = r.IsHidden,
            Title = r.Title.Trim(),
            Url = FormatUrl(r.Url),
            IsExternal = IsExternal(r.Url),
            PostId = r.PostId,
            LanguageId = r.LanguageId,
            ParentMenuId = r.ParentMenuId
        };
    }

    public override Menu UpdateEntity(CreateMenuRequest r, Menu e)
    {
        e.Order = r.Order;
        e.IsPublished = r.IsPublished;
        e.IsHidden = r.IsHidden;
        e.Title = r.Title.Trim();
        e.Url = FormatUrl(r.Url);
        e.IsExternal = IsExternal(r.Url);
        e.PostId = r.PostId;
        e.LanguageId = r.LanguageId;
        e.ParentMenuId = r.ParentMenuId;

        return e;
    }

    public override MenuResponse FromEntity(Menu e)
    {
        return new()
        {
            Id = e.Id,
            Order = e.Order,
            IsPublished = e.IsPublished,
            IsHidden = e.IsHidden,
            Title = e.Title,
            Url = e.Url,
            PostId = e.PostId,
            PostTitle = e.Post?.Title,
            LanguageName = e.Language.Name,
            LanguageId = e.LanguageId,
            ParentMenuId = e.ParentMenuId,
            Children = null
        };
    }

    public MenuResponse FromEntity(Menu e, List<MenuResponse> children)
    {
        return new()
        {
            Id = e.Id,
            Order = e.Order,
            IsPublished = e.IsPublished,
            IsHidden = e.IsHidden,
            Title = e.Title,
            Url = e.Url,
            PostId = e.PostId,
            LanguageName = e.Language.Name,
            LanguageId = e.LanguageId,
            ParentMenuId = e.ParentMenuId,
            Children = children.Count > 0 ? children : null
        };
    }

    public string? FormatUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        return $"/{url.Trim().TrimStart('/')}";
    }

    public bool IsExternal(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}