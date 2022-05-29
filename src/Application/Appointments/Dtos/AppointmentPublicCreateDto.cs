namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentCreateDto
{
    public int DateId { get; init; }

    public string UserName { get; init; } = default!;
}