namespace API.Endpoints.Events.Public.Month;

using API.Extensions;
using API.Services.Calendar;

public sealed class ListCalendarEventsMonthPublicEndpoint(
    ICalendarService calendarService,
    TimeProvider timeProvider)
    : Endpoint<ListCalendarEventsMonthPublicRequest, List<CalendarEvent>>
{
    public override void Configure()
    {
        Get("public/events/month/{offset:int:min(-1000):max(1000)}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        ListCalendarEventsMonthPublicRequest req, CancellationToken ct)
    {
        var now = timeProvider.LtNow;

        var start = now.AddDays(req.Offset * 31).Date;
        var end = start.AddDays(31 + 1);

        var events = await calendarService.ListEventsAsync(start, end, ct);

        await Send.OkAsync(events.ToList(), ct);
    }
}