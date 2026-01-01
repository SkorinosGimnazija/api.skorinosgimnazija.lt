namespace API.Endpoints.Events.Public.Now;

using API.Extensions;
using API.Services.Calendar;

public sealed class ListCalendarEventsNowPublicEndpoint(
    ICalendarService calendarService,
    TimeProvider timeProvider)
    : EndpointWithoutRequest<List<CalendarEvent>>
{
    public override void Configure()
    {
        Get("public/events/now");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var now = timeProvider.LtNow;

        var start = now.Date;
        var end = start.AddDays(1);

        var events = await calendarService.ListEventsAsync(start, end, ct);

        await Send.OkAsync(events.ToList(), ct);
    }
}