namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentTypeEditDto : AppointmentTypeCreateDto
{
    public int Id { get; init; }
}