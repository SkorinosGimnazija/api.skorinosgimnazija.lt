namespace SkorinosGimnazija.Application.Authorization.Dtos;

public record AuthDto
{
    public string Token { get; init; } = default!;
}