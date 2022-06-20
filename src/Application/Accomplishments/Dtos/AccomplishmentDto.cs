namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Achievement { get; init; } = default!;

    public DateTime Date { get; init; }

    public string TeacherDisplayName { get; init; } = default!;

    public int UserId { get; init; } = default!;

    public string Scale { get; init; } = default!;

    public ICollection<AccomplishmentTeacherDto> AdditionalTeachers { get; init; } = default!;

    public ICollection<AccomplishmentStudentDto> Students { get; init; } = default!;
}