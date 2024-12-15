namespace SkorinosGimnazija.Application.Common.Dtos;

public record IdNameDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
}