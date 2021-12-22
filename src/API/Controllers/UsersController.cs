namespace SkorinosGimnazija.API.Controllers;

using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.API.Controllers.Base;
using SkorinosGimnazija.Application.Events.Dtos;
using SkorinosGimnazija.Application.Events;
using SkorinosGimnazija.Infrastructure.Identity;
using System.Xml.Linq;
using Application.Common.Dtos;
using Application.Common.Identity;
using SkorinosGimnazija.Application.Users;

[Authorize(Roles = Auth.Role.Mod)]
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
