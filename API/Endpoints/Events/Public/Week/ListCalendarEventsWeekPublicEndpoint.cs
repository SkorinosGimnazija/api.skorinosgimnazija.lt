namespace API.Endpoints.Events.Public.Week;

using API.Extensions;
using API.Services.Calendar;

public sealed class ListCalendarEventsWeekPublicEndpoint(
    ICalendarService calendarService,
    TimeProvider timeProvider)
    : Endpoint<ListCalendarEventsWeekPublicRequest, List<CalendarEvent>>
{
    public override void Configure()
    {
        Get("public/events/week/{offset:int:min(-1000):max(1000)}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        ListCalendarEventsWeekPublicRequest req, CancellationToken ct)
    {
        var now = timeProvider.LtNow;

        var start = now.AddDays(req.Offset * 7).Date;
        var end = start.AddDays(7 + 1);

        var events = await calendarService.ListEventsAsync(start, end, ct);

        await Send.OkAsync(events.ToList(), ct);
    }
}