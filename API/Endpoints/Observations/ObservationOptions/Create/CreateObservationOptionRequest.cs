namespace API.Endpoints.Observations.ObservationOptions.Create;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.Observations;
using JetBrains.Annotations;

[PublicAPI]
public record CreateObservationOptionRequest
{
    [StringLength(ObservationOptionConfiguration.NameLength)]
    public required string Name { get; init; }
}