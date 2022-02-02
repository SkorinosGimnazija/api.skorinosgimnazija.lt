namespace SkorinosGimnazija.Domain.Options;

public record CalendarOptions
{
    public string AppointmentsCalendarId { get; init; } = default!;

    public string EventsCalendarId { get; init; } = default!;

    public string User { get; init; } = default!;
}