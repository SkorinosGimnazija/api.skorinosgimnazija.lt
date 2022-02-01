namespace SkorinosGimnazija.Application.Courses.Dtos;

public record CourseStatsDto
{
    public int UserId { get; init; }

    public string UserDisplayName { get; init; } = default!;

    public float Hours { get; init; }

    public int Count { get; init; }

    public int UsefulCount { get; init; }

    public float Price { get; init; }

    public DateTime LastUpdate { get; init; }
}