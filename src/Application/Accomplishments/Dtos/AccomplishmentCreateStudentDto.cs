namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentCreateStudentDto
{
    public int ClassroomId { get; init; }

    public int AchievementId { get; init; }

    public string Name { get; init; } = default!;
}