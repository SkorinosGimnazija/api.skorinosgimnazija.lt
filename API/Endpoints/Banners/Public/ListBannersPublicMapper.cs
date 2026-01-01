namespace API.Endpoints.Banners.Public;

using API.Database.Entities.CMS;

public sealed class ListBannersPublicMapper : ResponseMapper<ListBannersPublicResponse, Banner>
{
    public override ListBannersPublicResponse FromEntity(Banner e)
    {
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Url = e.Url,
            ImageUrl = e.ImageUrl,
            Width = e.Width,
            Height = e.Height
        };
    }
}