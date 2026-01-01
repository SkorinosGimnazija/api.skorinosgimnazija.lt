namespace API.Endpoints.Achievements.Stats;

using JetBrains.Annotations;

[PublicAPI]
public record GetAchievementStatsResponse
{
    public required int RecordsCount { get; init; }

    public required int StudentsCount { get; init; }

    public required int TeachersCount { get; init; }

    public required List<TypeStatsResponse> TypeStats { get; init; }

    [PublicAPI]
    public record TypeStatsResponse
    {
        public required int Id { get; init; }

        public required string Name { get; init; }

        public required int Count { get; init; }
    }
}