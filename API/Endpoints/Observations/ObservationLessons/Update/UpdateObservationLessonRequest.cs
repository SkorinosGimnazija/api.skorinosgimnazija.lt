namespace API.Endpoints.Observations.ObservationLessons.Update;

using API.Endpoints.Observations.ObservationLessons.Create;
using JetBrains.Annotations;

[PublicAPI]
public class UpdateObservationLessonRequest : CreateObservationLessonRequest
{
    public required int Id { get; init; }
}