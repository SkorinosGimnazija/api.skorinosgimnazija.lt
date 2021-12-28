namespace SkorinosGimnazija.Application.Courses.Validators;

using BullyReports.Dtos;
using Common.Interfaces;
using FluentValidation;
using Infrastructure.Captcha;

internal class BullyReportCreateValidator : AbstractValidator<BullyReportCreateDto>
{
    public BullyReportCreateValidator(ICaptchaService captchaService)
    {
        RuleFor(x => x.BullyInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.VictimInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Date).NotNull();
        RuleFor(x => x.ReporterInfo).MaximumLength(256);
        RuleFor(x => x.Details).MaximumLength(2048);
        RuleFor(x => x.CaptchaToken).SetValidator(new CaptchaValidator(captchaService));
    }
}