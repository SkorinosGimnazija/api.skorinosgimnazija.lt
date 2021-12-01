    namespace SkorinosGimnazija.Application.Banners.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Banners.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class BannerValidator : AbstractValidator<BannerCreateDto>
{
    public BannerValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(256);
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