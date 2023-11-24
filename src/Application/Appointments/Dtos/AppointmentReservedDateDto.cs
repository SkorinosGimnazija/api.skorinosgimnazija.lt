namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentReservedDateDto
{
    public int Id { get; init; }

    public int DateId { get; init; }
}