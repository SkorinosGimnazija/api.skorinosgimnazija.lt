namespace API.Endpoints.Appointments.AppointmentDates;

using API.Database.Entities.Appointments;

public class AppointmentDateMapper
    : ResponseMapper<AppointmentDateResponse, AppointmentDate>
{
    public override AppointmentDateResponse FromEntity(AppointmentDate e)
    {
        return new()
        {
            Id = e.Id,
            Date = e.Date
        };
    }
}