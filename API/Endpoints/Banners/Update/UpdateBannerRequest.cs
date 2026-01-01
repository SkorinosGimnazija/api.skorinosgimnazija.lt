namespace API.Endpoints.Banners.Update;

using API.Endpoints.Banners.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateBannerRequest : BannerRequest
{
    public required int Id { get; init; }

    public IFormFile? Image { get; init; }
}