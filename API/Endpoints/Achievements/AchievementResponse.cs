namespace API.Endpoints.Achievements;

using API.Endpoints.Users;
using JetBrains.Annotations;

[PublicAPI]
public record AchievementResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required DateOnly Date { get; init; }

    public required int CreatorId { get; init; }

    public required string CreatorName { get; init; }

    public required int ScaleId { get; init; }

    public required string ScaleName { get; init; }

    public required List<UserResponse> AdditionalTeachers { get; init; }

    public required List<AchievementStudentResponse> Students { get; init; }
}

[PublicAPI]
public record AchievementStudentResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int ClassroomId { get; init; }

    public required string ClassroomName { get; init; }

    public required int AchievementTypeId { get; init; }

    public required string AchievementTypeName { get; init; }
}