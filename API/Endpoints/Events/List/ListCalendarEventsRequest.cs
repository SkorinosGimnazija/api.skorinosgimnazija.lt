namespace API.Endpoints.Events.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListCalendarEventsRequest
{
    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }
}