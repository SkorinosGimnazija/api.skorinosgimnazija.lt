namespace API.Endpoints.Observations.ObservationLessons;

using API.Database.Entities.Observations;
using API.Endpoints.Observations.ObservationLessons.Create;

public sealed class ObservationLessonMapper
    : Mapper<CreateObservationLessonRequest, ObservationLessonResponse, ObservationLesson>
{
    public override ObservationLesson ToEntity(CreateObservationLessonRequest r)
    {
        return new()
        {
            Name = r.Name
        };
    }

    public override ObservationLessonResponse FromEntity(ObservationLesson e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }

    public override ObservationLesson UpdateEntity(
        CreateObservationLessonRequest r, ObservationLesson e)
    {
        e.Name = r.Name;

        return e;
    }
}