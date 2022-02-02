namespace SkorinosGimnazija.Application.BullyReports.Validators;

using Common.Interfaces;
using Common.Validators;
using Dtos;
using FluentValidation;

internal class BullyReportCreateValidator : AbstractValidator<BullyReportCreateDto>
{
    public BullyReportCreateValidator(ICaptchaService captchaService)
    {
        RuleFor(x => x.BullyInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.VictimInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Date).NotNull();
        RuleFor(x => x.Details).MaximumLength(2048);
        RuleFor(x => x.CaptchaToken).NotEmpty().SetValidator(new CaptchaValidator(captchaService));
    }
}