namespace SkorinosGimnazija.Application.Menus.Dtos;

using Languages.Dtos;
using Posts.Dtos;

public record MenuDetailsDto : MenuDto
{
    public LanguageDto Language { get; init; } = default!;

    public MenuLocationDto MenuLocation { get; init; } = default!;

    public PostDetailsDto? LinkedPost { get; set; }
}