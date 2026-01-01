namespace API.Endpoints.Achievements.Update;

using API.Endpoints.Achievements.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateAchievementRequest : CreateAchievementRequest
{
    public required int Id { get; init; }
}