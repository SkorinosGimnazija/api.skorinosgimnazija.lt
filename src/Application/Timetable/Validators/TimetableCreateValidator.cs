namespace SkorinosGimnazija.Application.Timetable.Validators;

using Dtos;
using FluentValidation;

internal class TimetableCreateValidator : AbstractValidator<TimetableCreateDto>
{
    public TimetableCreateValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.DayId).NotEmpty();
        RuleFor(x => x.TimeId).NotEmpty();
        RuleFor(x => x.ClassName).NotNull().MaximumLength(128);
    }
}