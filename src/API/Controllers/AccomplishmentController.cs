namespace SkorinosGimnazija.API.Controllers;

using Application.Accomplishments;
using Application.Accomplishments.Dtos;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Teacher)]
public class AccomplishmentController : BaseApiController
{
    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("all", Name = "GetAccomplishmentsByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AccomplishmentDto>> GetAll(
        [FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct)
    {
        return await Mediator.Send(new AccomplishmentAdminList.Query(start, end), ct);
    }

    [HttpGet("scales", Name = "GetAccomplishmentScales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AccomplishmentScaleDto>> GetScales(CancellationToken ct)
    {
        return await Mediator.Send(new AccomplishmentScalesList.Query(), ct);
    }

    [HttpGet("classrooms", Name = "GetAccomplishmentClassrooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AccomplishmentClassroomDto>> GetClassrooms(CancellationToken ct)
    {
        return await Mediator.Send(new AccomplishmentClassroomsList.Query(), ct);
    }

    [HttpGet(Name = "GetMyAccomplishments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AccomplishmentDto>> GetMy(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AccomplishmentList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetAccomplishmentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccomplishmentDetailsDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new AccomplishmentDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateAccomplishment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AccomplishmentDto>> Create(AccomplishmentCreateDto dto)
    {
        var result = await Mediator.Send(new AccomplishmentCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditAccomplishment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(AccomplishmentEditDto dto)
    {
        await Mediator.Send(new AccomplishmentEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteAccomplishment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new AccomplishmentDelete.Command(id));
        return NoContent();
    }
}