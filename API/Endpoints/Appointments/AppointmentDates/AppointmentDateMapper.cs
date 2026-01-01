namespace API.Endpoints.Appointments.AppointmentDates;

using API.Database.Entities.Appointments;
using API.Endpoints.Appointments.AppointmentDates.Update;

public class AppointmentDateMapper
    : Mapper<UpdateAppointmentDatesRequest, AppointmentDateResponse, AppointmentDate>
{
    public override AppointmentDateResponse FromEntity(AppointmentDate e)
    {
        return new()
        {
            Id = e.Id,
            Date = e.Date
        };
    }

    public override AppointmentDate ToEntity(UpdateAppointmentDatesRequest r)
    {
        return new()
        {
            Date = r.Date,
            TypeId = r.TypeId
        };
    }
}