namespace SkorinosGimnazija.Application.Appointments.Validators;

using Dtos;
using FluentValidation;

internal class AppointmentTypeCreateValidator : AbstractValidator<AppointmentTypeCreateDto>
{
    public AppointmentTypeCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DurationInMinutes).NotNull();
        RuleFor(x => x.Start).NotEmpty();
        RuleFor(x => x.End).NotEmpty();
        RuleFor(x => x.RegistrationEnd).NotEmpty();
    }
}