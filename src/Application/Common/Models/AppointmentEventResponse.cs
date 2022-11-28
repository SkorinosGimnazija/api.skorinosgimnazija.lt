namespace SkorinosGimnazija.Application.Common.Models;

public record AppointmentEventResponse
{
    public string EventId { get; init; } = default!;

    public string? EventMeetingLink { get; init; }
}