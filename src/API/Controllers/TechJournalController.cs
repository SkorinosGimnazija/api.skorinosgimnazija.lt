namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.TechJournal;
using Application.TechJournal.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Teacher)]
public class TechJournalController : BaseApiController
{
    [HttpGet(Name = "GetTechJournalReports")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<TechJournalReportDto>> GetAll(
        [FromQuery] PaginationDto pagination,
        [FromQuery] DateOnly start,
        [FromQuery] DateOnly end,
        CancellationToken ct)
    {
        return await Mediator.Send(new TechJournalReportList.Query(pagination, start, end), ct);
    }

    [HttpGet("{id:int}", Name = "GetTechJournalReportById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TechJournalReportDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new TechJournalReportDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateTechJournalReport")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TechJournalReportDto>> Create(TechJournalReportCreateDto dto)
    {
        var result = await Mediator.Send(new TechJournalReportCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditTechJournalReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(TechJournalReportEditDto dto)
    {
        await Mediator.Send(new TechJournalReportEdit.Command(dto));
        return Ok();
    }

    [Authorize(Roles = Auth.Role.TechManager)]
    [HttpPatch("{id:int}", Name = "PatchTechJournalReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(int id, TechJournalReportPatchDto dto)
    {
        await Mediator.Send(new TechJournalReportPatch.Command(id, dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteTechJournalReport")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new TechJournalReportDelete.Command(id));
        return NoContent();
    }
}