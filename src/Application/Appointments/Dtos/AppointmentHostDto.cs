namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentHostDto
{
    public string DisplayName { get; init; } = default!;

    public string UserName { get; init; } = default!;
}