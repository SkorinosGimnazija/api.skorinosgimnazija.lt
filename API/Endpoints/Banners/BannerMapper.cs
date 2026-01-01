namespace API.Endpoints.Banners;

using API.Database.Entities.CMS;
using API.Endpoints.Banners.Create;
using API.Endpoints.Banners.Update;

public sealed class BannerMapper
    : Mapper<CreateBannerRequest, BannerResponse, Banner>
{
    public override Banner ToEntity(CreateBannerRequest r)
    {
        return new()
        {
            Title = r.Title,
            IsPublished = r.IsPublished,
            LanguageId = r.LanguageId,
            Order = r.Order,
            Url = r.Url
        };
    }

    public Banner UpdateEntity(UpdateBannerRequest r, Banner e)
    {
        e.Title = r.Title;
        e.IsPublished = r.IsPublished;
        e.LanguageId = r.LanguageId;
        e.Order = r.Order;
        e.Url = r.Url;

        return e;
    }

    public override BannerResponse FromEntity(Banner e)
    {
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Height = e.Height,
            Width = e.Width,
            IsPublished = e.IsPublished,
            LanguageId = e.LanguageId,
            Language = e.Language.Name,
            Order = e.Order,
            Url = e.Url,
            ImageUrl = e.ImageUrl
        };
    }
}