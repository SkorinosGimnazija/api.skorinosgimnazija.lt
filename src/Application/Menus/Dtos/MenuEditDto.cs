namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuEditDto : MenuCreateDto
{
    public int Id { get; init; }
}