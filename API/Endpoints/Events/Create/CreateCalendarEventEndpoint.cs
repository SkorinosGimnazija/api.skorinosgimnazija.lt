namespace API.Endpoints.Events.Create;

using API.Services.Calendar;

public sealed class CreateCalendarEventEndpoint(ICalendarService calendarService)
    : Endpoint<List<CreateCalendarEventRequest>, List<CalendarResponse>>
{
    public override void Configure()
    {
        Post("events");
        PostProcessor<EventRevalidation<List<CreateCalendarEventRequest>, List<CalendarResponse>>>();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(List<CreateCalendarEventRequest> req, CancellationToken ct)
    {
        var requests = req.Select(item => new CalendarEventRequest
        {
            Title = item.Title,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            AllDay = item.AllDay
        }).ToList();

        var response = await calendarService.CreateEventAsync(requests, ct);

        await Send.ResponseAsync(response, StatusCodes.Status201Created, ct);
    }
}