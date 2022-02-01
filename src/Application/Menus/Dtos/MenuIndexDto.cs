namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuIndexDto
{
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Path { get; init; } = default!;
}