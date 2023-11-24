namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentExclusiveHostCreateDto
{
    public int TypeId { get; init; }

    public string UserName { get; init; } = default!;
}