namespace API.Services.Calendar;

public record CalendarResponse
{
    public required string EventId { get; init; }

    public string? EventLink { get; init; }
}