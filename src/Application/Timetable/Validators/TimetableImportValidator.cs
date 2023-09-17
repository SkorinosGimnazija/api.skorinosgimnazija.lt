namespace SkorinosGimnazija.Application.Timetable.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;

internal class TimetableImportValidator : AbstractValidator<TimetableImportDto>
{
    public TimetableImportValidator()
    {
        RuleFor(x => x.TimetableList).NotEmpty();
        RuleForEach(x => x.TimetableList).SetValidator(new TimetableCreateValidator());
    }
}
