namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentTeacherDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;
}