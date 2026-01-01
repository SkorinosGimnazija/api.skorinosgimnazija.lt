namespace API.Endpoints.Authorization;

using JetBrains.Annotations;

[PublicAPI]
public record AuthorizationResponse
{
    public required int Id { get; init; }

    public required string Token { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }

    public required List<string> Roles { get; init; }
}