namespace SkorinosGimnazija.Application.Timetable.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;

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
