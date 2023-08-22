namespace SkorinosGimnazija.Application.School.Dtos;

public record ClassroomDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public int Number { get; init; }
}