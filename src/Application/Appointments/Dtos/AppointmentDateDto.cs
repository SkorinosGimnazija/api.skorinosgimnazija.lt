namespace SkorinosGimnazija.Application.Appointments.Dtos;

using ParentAppointments.Dtos;

public record AppointmentDateDto
{
    public int Id { get; init; }

    public DateTime Date { get; init; }

    public AppointmentTypeDto Type { get; init; } = default!;
}