namespace API.Endpoints.Authorization.Login;

using JetBrains.Annotations;

[PublicAPI]
public record LoginRequest
{
    public required string Token { get; init; }
}