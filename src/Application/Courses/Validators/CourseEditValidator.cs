namespace SkorinosGimnazija.Application.Courses.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Menus.Dtos;

using SkorinosGimnazija.Application.Menus.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;

internal class CourseEditValidator : AbstractValidator<CourseEditDto>
{
    public CourseEditValidator()
    {
        Include(new CourseCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}