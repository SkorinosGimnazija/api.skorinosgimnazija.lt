namespace API.Endpoints.Appointments.AppointmentReservedDates.Update;

using API.Database.Entities.Appointments;

public sealed class UpdateAppointmentReservedDatesMapper
    : RequestMapper<UpdateAppointmentReservedDatesRequest, AppointmentReservedDate>
{
    public override AppointmentReservedDate ToEntity(UpdateAppointmentReservedDatesRequest r)
    {
        return new()
        {
            DateId = r.DateId,
            HostId = r.HostId
        };
    }
}