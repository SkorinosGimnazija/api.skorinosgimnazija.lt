namespace API.Endpoints.Achievements.Stats;

using JetBrains.Annotations;

[PublicAPI]
public record GetAchievementStatsRequest
{
    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }
}