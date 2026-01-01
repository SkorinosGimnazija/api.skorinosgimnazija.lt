namespace API.Endpoints.Events.Delete;

using API.Services.Calendar;

public sealed class DeleteCalendarEventEndpoint(ICalendarService calendarService)
    : Endpoint<RouteIdRequest<string>>
{
    public override void Configure()
    {
        Delete("events/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest<string> req, CancellationToken ct)
    {
        var deleted = await calendarService.DeleteEventAsync(req.Id);
        if (!deleted)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}