namespace API.Endpoints.Banners.Create;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.CMS;
using JetBrains.Annotations;

[PublicAPI]
public record CreateBannerRequest : BannerRequest
{
    public required IFormFile Image { get; init; }
}

[PublicAPI]
public record BannerRequest
{
    [StringLength(BannerConfiguration.TitleLength)]
    public required string Title { get; init; }

    [StringLength(BannerConfiguration.UrlLength)]
    public required string Url { get; init; }

    public required bool IsPublished { get; init; }

    public required int Order { get; init; }

    public required string LanguageId { get; init; }
}