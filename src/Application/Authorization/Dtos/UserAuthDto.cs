namespace SkorinosGimnazija.Application.Authorization.Dtos;

public record UserAuthDto
{
    public int Id { get; init; }

    public string Token { get; init; } = default!;

    public string DisplayName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public IEnumerable<string> Roles { get; init; } = default!;
}