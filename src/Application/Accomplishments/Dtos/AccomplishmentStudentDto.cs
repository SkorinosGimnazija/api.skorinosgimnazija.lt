namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentStudentDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public AccomplishmentClassroomDto Classroom { get; init; } = default!;

    public AccomplishmentAchievementDto Achievement { get; init; } = default!;
}