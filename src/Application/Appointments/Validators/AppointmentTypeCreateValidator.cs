namespace SkorinosGimnazija.Application.Appointments.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Infrastructure.Captcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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