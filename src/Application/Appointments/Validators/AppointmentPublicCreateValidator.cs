namespace SkorinosGimnazija.Application.ParentAppointments.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Infrastructure.Captcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AppointmentPublicCreateValidator : AbstractValidator<AppointmentPublicCreateDto>
{
    public AppointmentPublicCreateValidator(ICaptchaService captchaService)
    {
        RuleFor(x => x.AttendeeEmail).NotEmpty().MaximumLength(256).Must(BeGmail).WithMessage("Only @gmail.com");
        RuleFor(x => x.AttendeeName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateId).NotNull();
        RuleFor(x => x.CaptchaToken).NotEmpty().SetValidator(new CaptchaValidator(captchaService));
    } 

    private static bool BeGmail(string? email)
    {
        return email?.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) == true;
    }
}