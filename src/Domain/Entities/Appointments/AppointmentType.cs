namespace SkorinosGimnazija.Domain.Entities.Appointments;

public class AppointmentType
{
    public int Id { get; set; }

    public int DurationInMinutes { get; set; }

    public bool InvitePrincipal { get; set; }

    public bool IsPublic { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public DateTime RegistrationEnd { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}