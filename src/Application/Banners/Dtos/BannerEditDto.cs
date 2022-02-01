namespace SkorinosGimnazija.Application.Banners.Dtos;

using Microsoft.AspNetCore.Http;

public record BannerEditDto : BannerCreateDto
{
    public int Id { get; init; }

    public new IFormFile? Picture { get; init; }
}