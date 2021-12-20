namespace SkorinosGimnazija.Domain.Entities.Teacher;

using Identity;

public class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Organizer { get; set; } = default!;

    public DateOnly StartDate { get; set; } 

    public DateOnly EndDate { get; set; }

    public int DurationInHours { get; set; }

    public string? CertificateNr { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = default!;
}