namespace API.Endpoints.Observations.ObservationStudents.Update;

using API.Endpoints.Observations.ObservationStudents.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateObservationStudentRequest : CreateObservationStudentRequest
{
    public required int Id { get; init; }
}