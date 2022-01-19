namespace SkorinosGimnazija.API.Controllers;

using Base;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Appointments;
using SkorinosGimnazija.Infrastructure.Identity;
using System.Xml.Linq;
using SkorinosGimnazija.Application.Banners;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Appointments;
using SkorinosGimnazija.Application.Menus;
using SkorinosGimnazija.Application.MenuLocations;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Application.ParentAppointments;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;
using SkorinosGimnazija.Application.Courses.Dtos;
using SkorinosGimnazija.Application.Courses;

[Authorize(Roles = Auth.Role.Teacher)]
public class AppointmentsController : BaseApiController
{
    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("all", Name = "GetAllAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetAll([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAdminList.Query(pagination), ct);
    }

    [HttpGet(Name = "GetMyAppointments")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetMy([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentList.Query(pagination), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpGet("{id:int}", Name = "GetAppointmentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDetailsDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("{id:int}", Name = "DeleteAppointment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new AppointmentDelete.Command(id));
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("type/{id:int}", Name = "DeleteAppointmentType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteType(int id)
    {
        await Mediator.Send(new AppointmentTypeDelete.Command(id));
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut(Name = "EditAppointmentType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(AppointmentTypeEditDto dto)
    {
        await Mediator.Send(new AppointmentTypeEdit.Command(dto));
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpGet("types", Name = "GetAppointmentTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentTypeDto>> GetTypes(CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypesList.Query(), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpGet("type/{id:int}", Name = "GetAppointmentTypeById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentTypeDto>> GetType(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypeDetails.Query(id), ct);
    }


     
    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("type", Name = "CreateAppointmentType")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreateType(AppointmentTypeCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentTypeCreate.Command(dto));
        return CreatedAtAction(nameof(GetType), new { result.Id }, result);
    }

    [HttpPost("create", Name = "CreateAppointment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> Create(AppointmentCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpGet("time/{type}", Name = "GetAppointmentDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetDates(string type, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDatesList.Query(type), ct);
    }

    [HttpGet("time/{type}/{userName}", Name = "GetAppointmentAvailableDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetDates(string type, string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableDatesList.Query(type, userName, false), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/type/{slug}", Name = "GetPublicAppointmentTypeBySlug")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentTypeDto>> GetType(string slug, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypePublicDetails.Query(slug), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/time/{type}/{userName}", Name = "GetPublicAppointmentAvailableDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetPublicDates(string type, string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableDatesList.Query(type, userName, true), ct);
    }

    [AllowAnonymous]
    [HttpPost("public/create", Name = "CreatePublicAppointment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreatePublic(AppointmentPublicCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentPublicCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }
}
