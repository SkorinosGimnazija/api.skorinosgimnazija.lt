namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class AnnouncementCreateValidator : AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(512);
    }
}