namespace SkorinosGimnazija.Application.Courses.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyReports.Validators;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Courses.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BullyReportCreateValidator : AbstractValidator<BullyReportCreateDto>
{
    public BullyReportCreateValidator()
    {
        RuleFor(x => x.BullyInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.VictimInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.ReporterInfo).MaximumLength(256);
        RuleFor(x => x.Details).MaximumLength(2048);
    }
     
   
}
