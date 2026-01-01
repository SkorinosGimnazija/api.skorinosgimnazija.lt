namespace API.Endpoints.Achievements.Create;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.Achievements;
using JetBrains.Annotations;

[PublicAPI]
public record CreateAchievementRequest
{
    [StringLength(AchievementConfiguration.NameLength)]
    public required string Name { get; init; }

    public required DateOnly Date { get; init; }

    public required int ScaleId { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public required int CreatorId { get; init; }

    public required List<int> AdditionalTeachers { get; init; }

    public required List<AchievementStudentRequest> Students { get; init; }
}

[PublicAPI]
public record AchievementStudentRequest
{
    [StringLength(AchievementStudentConfiguration.NameLength)]
    public required string Name { get; init; }

    public required int AchievementTypeId { get; init; }

    public required int ClassroomId { get; init; }
}