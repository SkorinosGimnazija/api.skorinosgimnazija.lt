namespace API.Endpoints.Appointments.Entries.Public;

using API.Database.Entities.Appointments;
using API.Endpoints.Appointments.Entries.Create;
using API.Services.Captcha;
using JetBrains.Annotations;

[PublicAPI]
public record CreateAppointmentPublicRequest : CreateAppointmentRequest
{
    public required string Name { get; init; }

    public required string Note { get; init; }

    public required string Email { get; init; }

    public required string CaptchaToken { get; init; }
}

public class CreateAppointmentPublicRequestValidator : Validator<CreateAppointmentPublicRequest>
{
    public CreateAppointmentPublicRequestValidator()
    {
        Include(new CreateAppointmentRequestValidator());

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AppointmentConfiguration.AttendeeNameLength);

        RuleFor(x => x.Note)
            .NotEmpty()
            .MaximumLength(AppointmentConfiguration.NoteLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(AppointmentConfiguration.AttendeeEmailLength)
            .EmailAddress()
            .Must(Gmail)
            .WithMessage("El. paštas @gmail.com yra privalomas");

        RuleFor(x => x.CaptchaToken)
            .MustAsync(ValidCaptcha)
            .WithMessage("Captcha klaida");
    }

    private static bool Gmail(string? email)
    {
        return email?.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) == true;
    }

    private async Task<bool> ValidCaptcha(string? captcha, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(captcha))
        {
            return false;
        }

        var captchaService = Resolve<CaptchaService>();
        return await captchaService.ValidateAsync(captcha);
    }
}