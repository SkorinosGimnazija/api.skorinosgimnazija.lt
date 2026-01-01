namespace API.Endpoints.Courses.Stats;

using JetBrains.Annotations;

[PublicAPI]
public record GetCourseStatsRequest
{
    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }
}