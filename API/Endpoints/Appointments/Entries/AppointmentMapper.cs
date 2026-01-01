namespace API.Endpoints.Appointments.Entries;

using API.Database.Entities.Appointments;
using API.Endpoints.Appointments.Entries.Create;
using API.Endpoints.Appointments.Entries.Public;

public sealed class AppointmentMapper
    : Mapper<CreateAppointmentRequest, AppointmentResponse, Appointment>
{
    public override Appointment ToEntity(CreateAppointmentRequest req)
    {
        return new()
        {
            Id = Guid.CreateVersion7(),
            AppointmentDateId = req.DateId,
            HostId = req.HostId
        };
    }

    public Appointment ToEntity(CreateAppointmentPublicRequest req)
    {
        return new()
        {
            Id = Guid.CreateVersion7(),
            AppointmentDateId = req.DateId,
            HostId = req.HostId,
            AttendeeEmail = req.Name.Trim(),
            AttendeeName = req.Name.Trim(),
            Note = req.Note.Trim()
        };
    }

    public override AppointmentResponse FromEntity(Appointment e)
    {
        return new()
        {
            Id = e.Id,
            Link = e.Link,
            Date = e.AppointmentDate.Date,
            DateId = e.AppointmentDateId,
            TypeName = e.AppointmentDate.Type.Name,
            HostId = e.HostId,
            HostName = e.Host.Name,
            AttendeeName = e.AttendeeName,
            AttendeeEmail = e.AttendeeEmail,
            Note = e.Note
        };
    }
}