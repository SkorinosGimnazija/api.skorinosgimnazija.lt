namespace SkorinosGimnazija.Application.School.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ClasstimeShortDayEditValidator : AbstractValidator<ClasstimeShortDayEditDto>
{
    public ClasstimeShortDayEditValidator()
    {
        Include(new ClasstimeShortDayCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
