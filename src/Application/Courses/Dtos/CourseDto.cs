namespace SkorinosGimnazija.Application.Courses.Dtos;

public record CourseDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Organizer { get; init; } = default!;

    public DateTime StartDate { get; init; } = default!;

    public DateTime EndDate { get; init; } = default!;

    public DateTime CreatedAt { get; init; } = default!;

    public float DurationInHours { get; init; }

    public string? CertificateNr { get; init; }

    public float? Price { get; init; }

    public bool IsUseful { get; init; }

    public int UserId { get; init; }
}