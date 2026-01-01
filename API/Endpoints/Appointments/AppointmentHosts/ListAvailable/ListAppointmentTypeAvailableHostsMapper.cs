namespace API.Endpoints.Appointments.AppointmentHosts.ListAvailable;

using API.Database.Entities.Authorization;

public sealed class ListAppointmentTypeAvailableHostsMapper
    : ResponseMapper<AppointmentHostResponse, AppUser>
{
    public override AppointmentHostResponse FromEntity(AppUser e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            NormalizedName = e.NormalizedName
        };
    }
}