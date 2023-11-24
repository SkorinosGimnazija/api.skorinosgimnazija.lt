namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

using School.Dtos;

public record AccomplishmentStudentDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public ClassroomDto Classroom { get; init; } = default!;

    public AccomplishmentAchievementDto Achievement { get; init; } = default!;
}