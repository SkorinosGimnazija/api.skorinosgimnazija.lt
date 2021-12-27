namespace SkorinosGimnazija.Application.BullyReports.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Menus.Dtos;

using SkorinosGimnazija.Application.Menus.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Courses.Validators;
using Dtos;

internal class BullyReportEditValidator : AbstractValidator<BullyReportEditDto>
{
    public BullyReportEditValidator()
    {
        Include(new BullyReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}