namespace SkorinosGimnazija.Application.Appointments.Validators;

using FluentValidation;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Courses.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AppointmentTypeEditValidator : AbstractValidator<AppointmentTypeEditDto>
{
    public AppointmentTypeEditValidator()
    {
        Include(new AppointmentTypeCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
