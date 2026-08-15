namespace API.Endpoints.Events.Public.Month;

using JetBrains.Annotations;

[PublicAPI]
public record ListCalendarEventsMonthPublicRequest
{
    [RouteParam]
    public required int Offset { get; init; }
}