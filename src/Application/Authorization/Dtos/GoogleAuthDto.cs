namespace SkorinosGimnazija.Application.Authorization.Dtos;

public record GoogleAuthDto
{
    public string Token { get; init; } = default!;
}