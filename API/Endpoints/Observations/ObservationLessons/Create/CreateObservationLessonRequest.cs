namespace API.Endpoints.Observations.ObservationLessons.Create;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.Observations;
using JetBrains.Annotations;

[PublicAPI]
public class CreateObservationLessonRequest
{
    [StringLength(ObservationLessonConfiguration.NameLength)]
    public required string Name { get; init; }
}