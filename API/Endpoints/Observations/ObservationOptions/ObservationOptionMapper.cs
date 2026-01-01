namespace API.Endpoints.Observations.ObservationOptions;

using API.Database.Entities.Observations;
using API.Endpoints.Observations.ObservationOptions.Create;

public sealed class ObservationOptionMapper
    : Mapper<CreateObservationOptionRequest, ObservationOptionResponse, ObservationOption>
{
    public override ObservationOption ToEntity(CreateObservationOptionRequest r)
    {
        return new()
        {
            Name = r.Name
        };
    }

    public override ObservationOptionResponse FromEntity(ObservationOption e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }

    public override ObservationOption UpdateEntity(
        CreateObservationOptionRequest r, ObservationOption e)
    {
        e.Name = r.Name;

        return e;
    }
}