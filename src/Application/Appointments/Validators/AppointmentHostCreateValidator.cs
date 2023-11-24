namespace SkorinosGimnazija.Application.Appointments.Validators;

using Common.Interfaces;
using Dtos;
using FluentValidation;

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