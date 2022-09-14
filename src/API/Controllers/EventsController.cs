namespace SkorinosGimnazija.API.Controllers;

using Application.Events;
using Application.Events.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class EventsController : BaseApiController
{
    [HttpGet(Name = "GetEventsByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EventDto>> GetEvents(
        [FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct)
    {
        return await Mediator.Send(new EventList.Query(start, end), ct);
    }

    [HttpPost(Name = "CreateEvent")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventDto>> Create(EventCreateDto dto)
    {
        var result = await Mediator.Send(new EventCreate.Command(dto));
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpDelete("{id}", Name = "DeleteEvent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new EventDelete.Command(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("public/{week:int}", Name = "GetPublicEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EventDto>> GetAllEvents(int week, CancellationToken ct)
    {
        return await Mediator.Send(new EventPublicList.Query(week), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/today", Name = "GetPublicDayEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EventDto>> GetTodayEvents(CancellationToken ct)
    {
        return await Mediator.Send(new EventPublicList.Query(null), ct);
    }
}