namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentDto
{
    public int Id { get; init; }

    public string EventId { get; init; } = default!;

    public int DateId { get; init; }

    public string UserName { get; init; } = default!;

    public string AttendeeName { get; init; } = default!;

    public string AttendeeEmail { get; init; } = default!;
}