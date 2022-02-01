namespace SkorinosGimnazija.Application.ParentAppointments.Validators;

using Appointments.Dtos;
using FluentValidation;

internal class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
{
    public AppointmentCreateValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateId).NotNull();
    }
}