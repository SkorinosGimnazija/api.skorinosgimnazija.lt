namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Identity;
using Application.Users;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class UsersController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("public/teachers", Name = "GetPublicTeachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<TeacherDto>> GetTeachers(CancellationToken ct)
    {
        return await Mediator.Send(new TeachersPublicList.Query(), ct);
    }
}