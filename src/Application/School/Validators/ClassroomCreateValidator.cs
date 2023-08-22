namespace SkorinosGimnazija.Application.School.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;

internal class ClassroomCreateValidator : AbstractValidator<ClassroomCreateDto>
{
    public ClassroomCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(48);
        RuleFor(x => x.Number).NotNull();
    }
}
