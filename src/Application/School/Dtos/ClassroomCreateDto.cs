namespace SkorinosGimnazija.Application.School.Dtos;

public record ClassroomCreateDto
{
    public string Name { get; init; } = default!;

    public int Number { get; init; }
}