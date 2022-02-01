namespace SkorinosGimnazija.Application.ParentAppointments.Dtos;

public record AppointmentTypeDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public int DurationInMinutes { get; init; }

    public bool InvitePrincipal { get; init; }

    public bool IsPublic { get; init; }

    public DateTime Start { get; init; }

    public DateTime End { get; init; }

    public DateTime RegistrationEnd { get; init; }
}