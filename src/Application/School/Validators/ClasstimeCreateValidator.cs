namespace SkorinosGimnazija.Application.School.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ClasstimeCreateValidator : AbstractValidator<ClasstimeCreateDto>
{
    public ClasstimeCreateValidator()
    {
        RuleFor(x => x.Number).NotNull();
        RuleFor(x => x.StartTime).NotNull();
        RuleFor(x => x.EndTime).NotNull().GreaterThan(x => x.StartTime);
    }
}
