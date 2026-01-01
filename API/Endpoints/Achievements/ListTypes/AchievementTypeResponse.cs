namespace API.Endpoints.Achievements.ListTypes;

using JetBrains.Annotations;

[PublicAPI]
public record AchievementTypeResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}