namespace SkorinosGimnazija.Application.Appointments.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AppointmentHostCreateValidator : AbstractValidator<AppointmentExclusiveHostCreateDto>
{
    private readonly IEmployeeService _employeeService;

    public AppointmentHostCreateValidator(IEmployeeService employeeService)
    {
        _employeeService = employeeService;

        RuleFor(x => x.TypeId).NotEmpty();
        RuleFor(x => x.UserName).MustAsync(BeValidHost).WithMessage("Invalid host");
    }

    private async Task<bool> BeValidHost(string userName, CancellationToken ct)
    {
        return await _employeeService.GetEmployeeAsync(userName) is not null;
    }
}