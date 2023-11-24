namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentReservedDateCreateDto
{
    public int DateId { get; init; }

    public string UserName { get; init; } = default!;
}