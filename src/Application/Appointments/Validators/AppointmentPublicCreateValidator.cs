namespace SkorinosGimnazija.Application.Appointments.Validators;

using Common.Interfaces;
using Common.Validators;
using Dtos;
using FluentValidation;

internal class AppointmentPublicCreateValidator : AbstractValidator<AppointmentPublicCreateDto>
{
    public AppointmentPublicCreateValidator(ICaptchaService captchaService)
    {
        RuleFor(x => x.AttendeeEmail).EmailAddress().MaximumLength(256).Must(BeGmail).WithMessage("Tik @gmail.com");
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