﻿namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.Timetable;
using Application.Timetable.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Manager)]
public class TimetableController : BaseApiController
{
    [HttpGet(Name = "GetTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<TimetableDto>> GetList(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new TimetableList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetTimetableById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TimetableDto>> GetById(int id, CancellationToken ct)
    {
        return await Mediator.Send(new TimetableDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateTimetable")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TimetableDto>> Create(TimetableCreateDto dto)
    {
        var result = await Mediator.Send(new TimetableCreate.Command(dto));
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    [HttpPost("import", Name = "ImportTimetable")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Import(TimetableImportDto dto)
    {
        await Mediator.Send(new TimetableImport.Command(dto));
        return Ok();
    }

    [HttpPut(Name = "EditTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(TimetableEditDto dto)
    {
        await Mediator.Send(new TimetableEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteTimetable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new TimetableDelete.Command(id));
        return NoContent();
    }

    [HttpDelete("days", Name = "DeleteTimetableDay")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDay(TimetableDeleteDayDto dto)
    {
        await Mediator.Send(new TimetableDeleteDay.Command(dto));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("public/today", Name = "GetPublicTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<TimetablePublicDto?> GetTodayList(CancellationToken ct)
    {
        return await Mediator.Send(new TimetablePublicList.Query(), ct);
    }
}