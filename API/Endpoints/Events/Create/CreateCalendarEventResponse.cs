namespace API.Endpoints.Events.Create;

using JetBrains.Annotations;

[PublicAPI]
public record CreateCalendarEventResponse
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required DateTime StartDate { get; init; }

    public required DateTime EndDate { get; init; }
}