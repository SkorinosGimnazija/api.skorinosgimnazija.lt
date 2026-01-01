namespace API.Endpoints.Courses;

using JetBrains.Annotations;

[PublicAPI]
public record CourseResponse
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Organizer { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required double DurationInHours { get; init; }

    public string? Certificate { get; init; }

    public required bool IsUseful { get; init; }

    public required int CreatorId { get; init; }

    public required string CreatorName { get; init; }
}