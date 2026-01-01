namespace API.Endpoints.Observations.ObservationOptions;

using JetBrains.Annotations;

[PublicAPI]
public record ObservationOptionResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}