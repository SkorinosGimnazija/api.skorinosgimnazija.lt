namespace SkorinosGimnazija.Application.Banners.Validators;

using Dtos;
using FluentValidation;

internal class BannerValidator : AbstractValidator<BannerCreateDto>
{
    public BannerValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Width).NotNull();
        RuleFor(x => x.Height).NotNull();
        RuleFor(x => x.LanguageId).NotEmpty();
    }
}

internal class BannerPictureValidator : AbstractValidator<BannerCreateDto>
{
    public BannerPictureValidator()
    {
        RuleFor(x => x.Picture).NotNull();
    }
}