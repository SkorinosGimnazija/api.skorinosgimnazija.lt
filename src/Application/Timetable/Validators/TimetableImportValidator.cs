namespace SkorinosGimnazija.Application.Timetable.Validators;

using Dtos;
using FluentValidation;

internal class TimetableImportValidator : AbstractValidator<TimetableImportDto>
{
    public TimetableImportValidator()
    {
        RuleFor(x => x.TimetableList).NotEmpty();
        RuleForEach(x => x.TimetableList).SetValidator(new TimetableCreateValidator());
    }
}