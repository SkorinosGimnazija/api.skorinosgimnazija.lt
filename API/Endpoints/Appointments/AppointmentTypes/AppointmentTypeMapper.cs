namespace API.Endpoints.Appointments.AppointmentTypes;

using API.Database.Entities.Appointments;

public sealed class AppointmentTypeMapper
    : ResponseMapper<AppointmentTypeResponse, AppointmentType>
{
    public override AppointmentTypeResponse FromEntity(AppointmentType e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            RegistrationEndsAt = e.RegistrationEndsAt
        };
    }
}