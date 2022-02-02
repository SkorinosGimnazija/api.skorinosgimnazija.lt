namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Identity;
using Application.Employees;
using Application.Employees.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class EmployeesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("public/teachers", Name = "GetPublicTeachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EmployeeDto>> GetTeachers(CancellationToken ct)
    {
        return await Mediator.Send(new TeachersPublicList.Query(), ct);
    }
}