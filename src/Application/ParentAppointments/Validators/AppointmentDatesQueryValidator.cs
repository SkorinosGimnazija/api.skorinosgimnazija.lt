namespace SkorinosGimnazija.Application.ParentAppointments.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appointments;
using Common.Interfaces;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;
using MediatR;
using SkorinosGimnazija.Application.Common.Exceptions;

internal class AppointmentDatesQueryValidator : AbstractValidator<AppointmentDatesQuery>
{
    private readonly IEmployeeService _employeeService;

    public AppointmentDatesQueryValidator(IEmployeeService employeeService)
    {
        _employeeService = employeeService;

        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100).MustAsync(ValidEmployee);
        RuleFor(x => x.AppointmentTypeSlug).NotEmpty().MaximumLength(100);
    }

        private async Task<bool> ValidEmployee(string userName, CancellationToken ct)
        {
            var employee = await _employeeService.GetEmployeeAsync(userName);
            return employee is not null;
        }
}