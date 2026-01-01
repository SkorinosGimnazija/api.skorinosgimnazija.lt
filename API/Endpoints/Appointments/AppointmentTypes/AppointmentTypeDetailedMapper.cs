namespace API.Endpoints.Appointments.AppointmentTypes;

using API.Database.Entities.Appointments;
using API.Endpoints.Appointments.AppointmentTypes.Create;

public sealed class AppointmentTypeDetailedMapper
    : Mapper<CreateAppointmentTypeRequest, AppointmentTypeDetailedResponse, AppointmentType>
{
    public override AppointmentTypeDetailedResponse FromEntity(AppointmentType e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            DurationInMinutes = e.DurationInMinutes,
            IsPublic = e.IsPublic,
            IsOnline = e.IsOnline,
            RegistrationEndsAt = e.RegistrationEndsAt,
            AdditionalInviteeIds = e.AdditionalInvitees.Select(x => x.Id).ToList(),
            ExclusiveHostIds = e.ExclusiveHosts.Select(x => x.Id).ToList()
        };
    }

    public override AppointmentType ToEntity(CreateAppointmentTypeRequest r)
    {
        return new()
        {
            Name = r.Name,
            DurationInMinutes = r.DurationInMinutes,
            IsPublic = r.IsPublic,
            IsOnline = r.IsOnline,
            RegistrationEndsAt = r.RegistrationEndsAt
        };
    }

    public override AppointmentType UpdateEntity(CreateAppointmentTypeRequest r, AppointmentType e)
    {
        e.Name = r.Name;
        e.DurationInMinutes = r.DurationInMinutes;
        e.IsPublic = r.IsPublic;
        e.IsOnline = r.IsOnline;
        e.RegistrationEndsAt = r.RegistrationEndsAt;

        return e;
    }
}