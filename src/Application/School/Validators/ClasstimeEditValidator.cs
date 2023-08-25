namespace SkorinosGimnazija.Application.School.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ClasstimeEditValidator : AbstractValidator<ClasstimeEditDto>
{
    public ClasstimeEditValidator()
    {
        Include(new ClasstimeCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
