namespace API.Endpoints.School.Classdays;

using JetBrains.Annotations;

[PublicAPI]
public record ClassdayResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}