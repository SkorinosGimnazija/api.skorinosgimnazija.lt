namespace SkorinosGimnazija.Application.Appointments.Validators;

using Common.Interfaces;
using Dtos;
using FluentValidation;

internal class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
{
    public AppointmentCreateValidator(ICurrentUserService currentUserService)
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100).NotEqual(currentUserService.UserName);
        RuleFor(x => x.DateId).NotNull();
    }
}