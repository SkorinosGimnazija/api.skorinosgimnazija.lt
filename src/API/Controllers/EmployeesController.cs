namespace SkorinosGimnazija.API.Controllers;

using Application.Employees;
using Application.Employees.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Teacher)]
public class EmployeesController : BaseApiController
{
    [HttpGet("teachers", Name = "GetTeachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EmployeeDto>> GetTeachers(CancellationToken ct)
    {
        return await Mediator.Send(new TeachersList.Query(), ct);
    }
}