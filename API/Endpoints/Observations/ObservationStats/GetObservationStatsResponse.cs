namespace API.Endpoints.Observations.ObservationStats;

using JetBrains.Annotations;

[PublicAPI]
public record GetObservationStatsResponse
{
    public required List<TeacherRecordCount> TeacherRecordsCount { get; init; }

    public required Dictionary<string, Dictionary<int, int>> Stats { get; init; }

    public required List<Note> Notes { get; init; }

    [PublicAPI]
    public record TeacherRecordCount
    {
        public required string Id { get; init; }

        public required string Name { get; init; }

        public required int Count { get; init; }
    }

    [PublicAPI]
    public record Note
    {
        public required int Id { get; init; }

        public required string Text { get; init; }

        public required string CreatorName { get; init; }
    }
}