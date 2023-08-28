namespace SkorinosGimnazija.Application.Timetable.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.BullyReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;
using SkorinosGimnazija.Application.BullyJournal.Validators;

internal class TimetableEditValidator : AbstractValidator<TimetableEditDto>
{
    public TimetableEditValidator()
    {
        Include(new TimetableCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
