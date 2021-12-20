﻿namespace SkorinosGimnazija.Application.Courses.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Courses.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class CourseCreateValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Organizer).NotEmpty().MaximumLength(256);
        RuleFor(x => x.CertificateNr).MaximumLength(100);
    }
}
