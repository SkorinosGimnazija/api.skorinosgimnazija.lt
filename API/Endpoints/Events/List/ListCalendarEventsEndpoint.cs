namespace API.Endpoints.Events.List;

using API.Services.Calendar;

public sealed class ListCalendarEventsEndpoint(ICalendarService calendarService)
    : Endpoint<ListCalendarEventsRequest, List<CalendarEvent>>
{
    public override void Configure()
    {
        Get("events");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListCalendarEventsRequest req, CancellationToken ct)
    {
        var start = req.StartDate.ToDateTime(TimeOnly.MinValue);
        var end = req.EndDate.ToDateTime(TimeOnly.MaxValue);

        var events = await calendarService.ListEventsAsync(start, end, ct);

        await Send.OkAsync(events.Reverse().ToList(), ct);
    }
}