namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentCreateDto
{
    public int DateId { get; set; }

    public string UserName { get; set; } = default!;
}