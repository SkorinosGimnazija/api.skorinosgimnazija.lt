namespace SkorinosGimnazija.Application.School.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AnnouncementCreateValidator : AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(512);
    }
}
