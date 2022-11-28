namespace SkorinosGimnazija.Application.Common.Models;

public record AppointmentEvent
{
    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public bool IsOnline { get; init; }

    public IReadOnlyCollection<string> AttendeeEmails { get; init; } = default!;
}