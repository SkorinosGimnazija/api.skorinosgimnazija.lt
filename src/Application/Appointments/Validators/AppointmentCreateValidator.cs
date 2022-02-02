namespace SkorinosGimnazija.Application.Appointments.Validators;

using Dtos;
using FluentValidation;

internal class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
{
    public AppointmentCreateValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateId).NotNull();
    }
}