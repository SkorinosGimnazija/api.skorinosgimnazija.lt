namespace SkorinosGimnazija.Application.Appointments.Validators;

using Dtos;
using FluentValidation;

internal class AppointmentTypeEditValidator : AbstractValidator<AppointmentTypeEditDto>
{
    public AppointmentTypeEditValidator()
    {
        Include(new AppointmentTypeCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}