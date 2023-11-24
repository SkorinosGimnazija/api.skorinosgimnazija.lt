namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentExclusiveHostDto
{
    public int Id { get; init; }

    public int TypeId { get; init; }

    public string UserName { get; init; } = default!;
}