namespace API.Endpoints.Users;

using JetBrains.Annotations;

[PublicAPI]
public record UserResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string NormalizedName { get; init; }
}