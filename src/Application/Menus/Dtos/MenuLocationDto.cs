namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuLocationDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Slug { get; init; } = default!;
}