namespace API.Services.Calendar;

using JetBrains.Annotations;

[PublicAPI]
public record CalendarEvent
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string StartDate { get; init; }

    public required string EndDate { get; init; }

    public required bool AllDay { get; init; }
}