namespace SkorinosGimnazija.Application.Courses.Dtos;

public record CourseStats
{
    public string Name { get; init; } = default!;

    public float Hours { get; init; }

    public float? Price { get; init; }

    public DateTime LastUpdate { get; init; }
}