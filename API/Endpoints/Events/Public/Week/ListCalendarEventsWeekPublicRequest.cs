namespace API.Endpoints.Events.Public.Week;

using JetBrains.Annotations;

[PublicAPI]
public record ListCalendarEventsWeekPublicRequest
{
    [RouteParam]
    public required int Offset { get; init; }
}