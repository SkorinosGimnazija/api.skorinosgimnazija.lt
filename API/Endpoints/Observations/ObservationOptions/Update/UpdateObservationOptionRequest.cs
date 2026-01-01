namespace API.Endpoints.Observations.ObservationOptions.Update;

using API.Endpoints.Observations.ObservationOptions.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateObservationOptionRequest : CreateObservationOptionRequest
{
    public required int Id { get; init; }
}