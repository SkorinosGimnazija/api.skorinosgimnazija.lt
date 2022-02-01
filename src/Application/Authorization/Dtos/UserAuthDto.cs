namespace SkorinosGimnazija.Application.Authorization.Dtos;

public record UserAuthDto
{
    public string Token { get; init; } = default!;

    public string DisplayName { get; init; } = default!;

    public IEnumerable<string> Roles { get; init; } = default!;
}