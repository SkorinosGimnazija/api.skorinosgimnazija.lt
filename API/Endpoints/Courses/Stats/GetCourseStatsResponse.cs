namespace API.Endpoints.Courses.Stats;

using JetBrains.Annotations;

[PublicAPI]
public record GetCourseStatsResponse
{
    public required int RecordsCount { get; init; }

    public required double TotalDuration { get; init; }

    public required List<TeacherStatsResponse> Teachers { get; init; }

    [PublicAPI]
    public record TeacherStatsResponse
    {
        public required int Id { get; init; }

        public required string Name { get; init; }

        public required int RecordsCount { get; init; }

        public required double TotalDuration { get; init; }
    }
}