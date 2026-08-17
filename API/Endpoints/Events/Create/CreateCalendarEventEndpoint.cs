namespace API.Endpoints.Events.Create;

using API.Services.Calendar;

public sealed class CreateCalendarEventEndpoint(ICalendarService calendarService)
    : Endpoint<CreateCalendarEventRequest, CreateCalendarEventResponse>
{
    public override void Configure()
    {
        Post("events");
        PostProcessor<EventRevalidation<CreateCalendarEventRequest, CreateCalendarEventResponse>>();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateCalendarEventRequest req, CancellationToken ct)
    {
        var request = new CalendarEventRequest
        {
            Title = req.Title,
            StartDate = req.StartDate,
            EndDate = req.EndDate,
            AllDay = req.AllDay
        };

        var response = await calendarService.CreateEventAsync(request, ct);

        await Send.ResponseAsync(new()
        {
            Id = response.EventId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Title = request.Title
        }, StatusCodes.Status201Created, ct);
    }
}