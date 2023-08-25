namespace SkorinosGimnazija.API.Controllers;

using Application.Accomplishments;
using Application.Accomplishments.Dtos;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.School;
using SkorinosGimnazija.Application.School.Dtos;
using SkorinosGimnazija.Application.TechJournal.Dtos;
using SkorinosGimnazija.Application.TechJournal;
using SkorinosGimnazija.Application.Courses;

[Authorize(Roles = Auth.Role.Teacher)]
public class SchoolController : BaseApiController
{
    [HttpGet("classrooms", Name = "GetClassrooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ClassroomDto>> GetClassrooms(CancellationToken ct)
    {
        return await Mediator.Send(new ClassroomList.Query(), ct);
    }

    [HttpGet("classrooms/{id:int}", Name = "GetClassroomById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassroomDto>> GetClassroom(int id, CancellationToken ct)
    {
        return await Mediator.Send(new ClassroomDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("classrooms", Name = "CreateClassroom")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClassroomDto>> CreateClassroom(ClassroomCreateDto dto)
    {
        var result = await Mediator.Send(new ClassroomCreate.Command(dto));
        return CreatedAtAction(nameof(GetClassroom), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("classrooms", Name = "EditClassroom")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditClassroom(ClassroomEditDto dto)
    {
        await Mediator.Send(new ClassroomEdit.Command(dto));
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("classrooms/{id:int}", Name = "DeleteClassroom")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClassroom(int id)
    {
        await Mediator.Send(new ClassroomDelete.Command(id));
        return NoContent();
    }

    [HttpGet("classtimes", Name = "GetClasstimes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ClasstimeDto>> GetClassTimes(CancellationToken ct)
    {
        return await Mediator.Send(new ClasstimeList.Query(), ct);
    }

    [HttpGet("classtimes/{id:int}", Name = "GetClasstimeById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClasstimeDto>> GetClasstime(int id, CancellationToken ct)
    {
        return await Mediator.Send(new ClasstimeDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("classtimes", Name = "CreateClasstime")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClasstimeDto>> CreateClasstime(ClasstimeCreateDto dto)
    {
        var result = await Mediator.Send(new ClasstimeCreate.Command(dto));
        return CreatedAtAction(nameof(GetClasstime), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("classtime", Name = "EditClasstime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditClasstime(ClasstimeEditDto dto)
    {
        await Mediator.Send(new ClasstimeEdit.Command(dto));
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("classtimes/{id:int}", Name = "DeleteClasstime")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClasstime(int id)
    {
        await Mediator.Send(new ClasstimeDelete.Command(id));
        return NoContent();
    }
}