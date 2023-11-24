namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class AnnouncementEditValidator : AbstractValidator<AnnouncementEditDto>
{
    public AnnouncementEditValidator()
    {
        Include(new AnnouncementCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}