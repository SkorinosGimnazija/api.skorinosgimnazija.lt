namespace SkorinosGimnazija.Application.Banners.Validators;

using Dtos;
using FluentValidation;

internal class BannerCreateValidator : AbstractValidator<BannerCreateDto>
{
    public BannerCreateValidator()
    {
        Include(new BannerValidator());
        Include(new BannerPictureValidator());
    }
}