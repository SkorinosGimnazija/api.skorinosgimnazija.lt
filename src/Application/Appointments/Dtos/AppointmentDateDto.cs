namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentDateDto
{
    public int Id { get; init; }

    public DateTime Date { get; init; }

    public AppointmentTypeDto Type { get; init; } = default!;
}