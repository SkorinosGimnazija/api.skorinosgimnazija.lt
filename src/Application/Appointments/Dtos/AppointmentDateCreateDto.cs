namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentDateCreateDto
{
    public int TypeId { get; init; }

    public DateTime Date { get; init; }
}