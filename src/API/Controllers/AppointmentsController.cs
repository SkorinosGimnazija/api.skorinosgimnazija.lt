﻿namespace SkorinosGimnazija.API.Controllers;

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
    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("all", Name = "GetAllAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetAll(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAdminList.Query(pagination), ct);
    }

    [HttpGet(Name = "GetMyAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDetailsDto>> GetMy(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
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

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("time", Name = "CreateAppointmentDate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDateDto>> CreateDate(AppointmentDateCreateDto dto)
    {
        var result = await Mediator.Send(new AppointmentDateCreate.Command(dto));
        return CreatedAtAction(nameof(GetDates), new { dto.TypeId }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("time/{id:int}", Name = "DeleteAppointmentDate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDate(int id)
    {
        await Mediator.Send(new AppointmentDateDelete.Command(id));
        return NoContent();
    }

    [HttpGet("time/{typeId:int}", Name = "GetAppointmentDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetDates(int typeId, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDatesList.Query(typeId), ct);
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