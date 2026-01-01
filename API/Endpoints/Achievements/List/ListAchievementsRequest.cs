namespace API.Endpoints.Achievements.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListAchievementsRequest : PaginationRequest
{
    [QueryParam]
    public int? UserId { get; init; }
}