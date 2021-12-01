namespace SkorinosGimnazija.Application.Languages.Dtos;

public record LanguageDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Slug { get; init; } = default!;
}