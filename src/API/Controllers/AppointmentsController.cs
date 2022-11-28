namespace SkorinosGimnazija.API.Controllers;

using Application.Appointments;
using Application.Appointments.Dtos;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Teacher)]
public class AppointmentsController : BaseApiController
{
    [HttpGet("my/appointments/{typeSlug}", Name = "GetMyAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetAppointments(
        [FromQuery] PaginationDto pagination, string typeSlug, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentToUserList.Query(typeSlug,pagination), ct);
    }

    [HttpGet("my/registrations/{typeSlug}", Name = "GetMyRegistrations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetRegistrations(
        [FromQuery] PaginationDto pagination, string typeSlug, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentFromUserList.Query(typeSlug,pagination), ct);
    }

    [HttpPost("create", Name = "CreateAppointment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpDelete("{id:int}", Name = "DeleteAppointment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new AppointmentDelete.Command(id));
        return NoContent();
    }

    [HttpGet("hosts/available/{typeSlug}", Name = "GetAppointmentAvailableHosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentHostDto>> GetAvailableHosts(string typeSlug, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableHostsList.Query(typeSlug, false), ct);
    }

    [HttpGet("dates/available/{typeSlug}/{userName}", Name = "GetAppointmentAvailableDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetAvailableDates(string typeSlug, string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableDatesList.Query(typeSlug, userName, false), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet(Name = "GetAllAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetAll(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAdminList.Query(pagination), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("{id:int}", Name = "GetAppointmentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDetailsDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("types/{id:int}", Name = "DeleteAppointmentType")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteType(int id)
    {
        await Mediator.Send(new AppointmentTypeDelete.Command(id));
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("types", Name = "EditAppointmentType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(AppointmentTypeEditDto dto)
    {
        await Mediator.Send(new AppointmentTypeEdit.Command(dto));
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("types", Name = "GetAppointmentTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentTypeDto>> GetTypes(CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypesList.Query(), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("types/{id:int}", Name = "GetAppointmentTypeById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentTypeDto>> GetType(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypeDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("types", Name = "CreateAppointmentType")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreateType(AppointmentTypeCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentTypeCreate.Command(dto));
        return CreatedAtAction(nameof(GetType), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("hosts", Name = "CreateAppointmentHost")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentExclusiveHostDto>> CreateHost(AppointmentExclusiveHostCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentHostCreate.Command(dto));
        return CreatedAtAction(nameof(GetDates), new { dto.TypeId }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("hosts/{id:int}", Name = "DeleteAppointmentHost")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHost(int id)
    {
        await Mediator.Send(new AppointmentHostDelete.Command(id));
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("hosts/{typeId:int}", Name = "GetAppointmentHosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentExclusiveHostDto>> GetHosts(int typeId, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentHostsList.Query(typeId), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("dates", Name = "CreateAppointmentDate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDateDto>> CreateDate(AppointmentDateCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentDateCreate.Command(dto));
        return CreatedAtAction(nameof(GetDates), new { dto.TypeId }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("dates/{id:int}", Name = "DeleteAppointmentDate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDate(int id)
    {
        await Mediator.Send(new AppointmentDateDelete.Command(id));
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("dates/{typeId:int}", Name = "GetAppointmentDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentDateDto>> GetDates(int typeId, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDatesList.Query(typeId), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("dates/reserved/{userName}", Name = "GetAppointmentReservedDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentReservedDateDto>> GetReservedDates(string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentReservedDatesList.Query(userName), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpPost("dates/reserved", Name = "CreateAppointmentReservedDate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentReservedDateDto>> CreateReservedDate(AppointmentReservedDateCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentReservedDateCreate.Command(dto));
        return CreatedAtAction(nameof(GetReservedDates), new { dto.UserName }, result);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpDelete("dates/reserved/{id:int}", Name = "DeleteAppointmentReservedDate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReservedDate(int id)
    {
        await Mediator.Send(new AppointmentReservedDateDelete.Command(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("public/hosts/available/{typeSlug}", Name = "GetPublicAppointmentAvailableHosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentHostDto>> GetPublicAvailableHosts(string typeSlug, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableHostsList.Query(typeSlug, true), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/dates/available/{typeSlug}/{userName}", Name = "GetPublicAppointmentAvailableDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetPublicAvailableDates(string typeSlug, string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAvailableDatesList.Query(typeSlug, userName, true), ct);
    }

    [AllowAnonymous]
    [HttpPost("public/create", Name = "CreatePublicAppointment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreatePublicAppointment(AppointmentPublicCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentPublicCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }
}