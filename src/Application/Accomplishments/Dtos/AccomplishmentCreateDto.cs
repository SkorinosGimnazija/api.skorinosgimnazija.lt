namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

using SkorinosGimnazija.Domain.Entities.Accomplishments;

public record AccomplishmentCreateDto
{
    public string Name { get; init; } = default!;

    public string Achievement { get; init; } = default!;

    public DateTime Date { get; init; }

    public int ScaleId { get; init; }

    public ICollection<AccomplishmentCreateTeacherDto> AdditionalTeachers { get; init; } = default!;

    public ICollection<AccomplishmentCreateStudentDto> Students { get; init; } = default!;
}