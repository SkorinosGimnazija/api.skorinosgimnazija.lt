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
    //[AllowAnonymous]
    [HttpGet("public/{week:int}", Name = "GetPublicEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EventDto>> GetAllEvents(int week, CancellationToken ct)
    {
        return await Mediator.Send(new EventList.Query(week), ct);
    }

    //[AllowAnonymous]
    [HttpGet("public/today", Name = "GetPublicDayEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<EventDto>> GetTodayEvents(CancellationToken ct)
    {
        return await Mediator.Send(new EventList.Query(null), ct);
    }
}