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
using SkorinosGimnazija.Application.Timetable.Dtos;
using SkorinosGimnazija.Application.Timetable;
using SkorinosGimnazija.Domain.Entities.School;

[Authorize(Roles = Auth.Role.Manager)]
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

    [HttpGet("classdays", Name = "GetClassdays")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ClassdayDto>> GetClassdays(CancellationToken ct)
    {
        return await Mediator.Send(new ClassdayList.Query(), ct);
    }

    [HttpGet("announcements/{id:int}", Name = "GetAnnouncementById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AnnouncementDetails.Query(id), ct);
    }

    [HttpPost("announcements", Name = "CreateAnnouncement")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnnouncementDto>> CreateAnnouncement(AnnouncementCreateDto dto)
    {
        var result = await Mediator.Send(new AnnouncementCreate.Command(dto));
        return CreatedAtAction(nameof(GetAnnouncement), new { result.Id }, result);
    }

    [HttpPut("announcements", Name = "EditAnnouncement")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditAnnouncement(AnnouncementEditDto dto)
    {
        await Mediator.Send(new AnnouncementEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("announcements/{id:int}", Name = "DeleteAnnouncement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        await Mediator.Send(new AnnouncementDelete.Command(id));
        return NoContent();
    }

    [HttpGet("announcements", Name = "GetAnnouncements")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<PaginatedList<AnnouncementDto>> GetAnnouncements(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AnnouncementList.Query(pagination), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/announcements", Name = "GetPublicAnnouncements")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AnnouncementDto>> GetPublicAnnouncements(CancellationToken ct)
    {
        return await Mediator.Send(new AnnouncementsPublicList.Query(), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/random-image", Name = "GetPublicRandomImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<RandomImageDto?> GetPublicRandomImage(CancellationToken ct)
    {
        return await Mediator.Send(new RandomImage.Query(), ct);
    }
}