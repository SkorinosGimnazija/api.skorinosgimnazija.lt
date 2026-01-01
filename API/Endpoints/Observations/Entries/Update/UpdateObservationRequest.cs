namespace API.Endpoints.Observations.Entries.Update;

using API.Endpoints.Observations.Entries.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateObservationRequest : CreateObservationRequest
{
    public required int Id { get; init; }
}