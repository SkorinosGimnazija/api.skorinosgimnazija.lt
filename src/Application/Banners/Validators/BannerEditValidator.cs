namespace SkorinosGimnazija.Application.Banners.Validators;

using Dtos;
using FluentValidation;

internal class BannerEditValidator : AbstractValidator<BannerEditDto>
{
    public BannerEditValidator()
    {
        Include(new BannerValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}