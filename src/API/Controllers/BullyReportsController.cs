namespace SkorinosGimnazija.API.Controllers;

using Application.BullyReports;
using Application.BullyReports.Dtos;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.BullyManager)]
public class BullyReportsController : BaseApiController
{
    [HttpGet(Name = "GetBullyReports")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<BullyReportDto>> GetAll([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new BullyReportList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetBullyReportById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BullyReportDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new BullyReportDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("{id:int}", Name = "DeleteBullyReport")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new BullyReportDelete.Command(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("public", Name = "CreatePublicBullyReport")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BullyReportDto>> Create(BullyReportCreateDto dto)
    {
        var result = await Mediator.Send(new BullyReportPublicCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }
}