namespace API.Endpoints.Observations.ObservationStudents;

using API.Database.Entities.Observations;
using API.Endpoints.Observations.ObservationStudents.Create;

public sealed class ObservationStudentMapper
    : Mapper<CreateObservationStudentRequest, ObservationStudentResponse, ObservationStudent>
{
    public override ObservationStudent ToEntity(CreateObservationStudentRequest r)
    {
        return new()
        {
            Name = r.Name,
            IsActive = r.IsActive
        };
    }

    public override ObservationStudentResponse FromEntity(ObservationStudent e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            IsActive = e.IsActive
        };
    }

    public override ObservationStudent UpdateEntity(
        CreateObservationStudentRequest r, ObservationStudent e)
    {
        e.Name = r.Name;
        e.IsActive = r.IsActive;

        return e;
    }
}