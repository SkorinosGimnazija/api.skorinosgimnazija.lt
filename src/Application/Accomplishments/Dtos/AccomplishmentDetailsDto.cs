namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentDetailsDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Achievement { get; init; } = default!;

    public DateTime Date { get; init; }

    public int UserId { get; init; } = default!;

    public int ScaleId { get; init; } = default!;

    public ICollection<AccomplishmentTeacherDto> AdditionalTeachers { get; init; } = default!;

    public ICollection<AccomplishmentStudentDto> Students { get; init; } = default!;
}