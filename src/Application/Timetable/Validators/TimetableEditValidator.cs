namespace SkorinosGimnazija.Application.Timetable.Validators;

using Dtos;
using FluentValidation;

internal class TimetableEditValidator : AbstractValidator<TimetableEditDto>
{
    public TimetableEditValidator()
    {
        Include(new TimetableCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}