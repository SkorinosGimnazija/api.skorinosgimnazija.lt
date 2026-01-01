namespace API.Services.Calendar;

public record CalendarEventRequest
{
    public required string Title { get; init; }

    public required DateTime StartDate { get; init; }

    public required DateTime EndDate { get; init; }

    public required bool AllDay { get; init; }
}