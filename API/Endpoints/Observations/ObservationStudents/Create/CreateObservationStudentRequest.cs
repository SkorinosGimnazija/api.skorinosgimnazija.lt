namespace API.Endpoints.Observations.ObservationStudents.Create;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.Observations;
using JetBrains.Annotations;

[PublicAPI]
public record CreateObservationStudentRequest
{
    [StringLength(ObservationStudentConfiguration.NameLength)]
    public required string Name { get; init; }

    public required bool IsActive { get; init; }
}