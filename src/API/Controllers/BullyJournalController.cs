namespace SkorinosGimnazija.API.Controllers;

using Application.Banners;
using Application.BullyJournal;
using Application.BullyJournal.Dtos;
using Application.BullyJournalReports;
using Application.BullyReports;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.Accomplishments.Dtos;
using SkorinosGimnazija.Application.Accomplishments;
using SkorinosGimnazija.Application.BullyReports.Dtos;

[Authorize(Roles = Auth.Role.Teacher)]
public class BullyJournalController : BaseApiController
{
    [HttpGet(Name = "GetBullyJournalReports")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<BullyJournalReportDto>> GetAll(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new BullyJournalReportList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetBullyJournalReportById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BullyJournalReportDetailsDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new BullyJournalReportDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateBullyJournalReport")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BullyJournalReportDetailsDto>> Create(BullyJournalReportCreateDto dto)
    {
        var result = await Mediator.Send(new BullyJournalReportCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditBullyJournalReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(BullyJournalReportEditDto dto)
    {
        await Mediator.Send(new BullyJournalReportEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteBullyJournalReport")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new BullyJournalReportDelete.Command(id));
        return NoContent();
    }
}