namespace API.Endpoints.Achievements.ListScales;

using JetBrains.Annotations;

[PublicAPI]
public record AchievementScaleResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}