namespace API.Endpoints.Appointments.Entries.Create;

using API.Database.Entities.Appointments;
using API.Extensions;
using EntityFramework.Exceptions.Common;
using Npgsql;

public sealed class CreateAppointmentEndpoint(AppDbContext dbContext)
    : Endpoint<CreateAppointmentRequest, AppointmentResponse, AppointmentMapper>
{
    public override void Configure()
    {
        Post("appointments");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateAppointmentRequest req, CancellationToken ct)
    {
        try
        {
            var attendee = await dbContext.Users.AsNoTracking()
                               .Select(x => new { x.Id, x.Name, x.Email })
                               .FirstAsync(x => x.Id == User.GetId(), ct);

            var appointment = Map.ToEntity(req);
            appointment.AttendeeName = attendee.Name;
            appointment.AttendeeEmail = attendee.Email;

            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync(ct);

            await new CreateCalendarAppointmentCommand { AppointmentId = appointment.Id }
                .QueueJobAsync(ct: ct);

            appointment = await dbContext.Appointments
                              .Include(x => x.Host)
                              .Include(x => x.AppointmentDate)
                              .Include(x => x.AppointmentDate.Type)
                              .FirstAsync(x => x.Id == appointment.Id, ct);

            await Send.ResponseAsync(Map.FromEntity(appointment), StatusCodes.Status201Created, ct);
        }
        catch (UniqueConstraintException e) when (e.InnerException is PostgresException pe)
        {
            e.Data[nameof(pe.MessageText)] =
                AppointmentConfiguration.GetErrorMessage(pe.ConstraintName);
            throw;
        }
    }
}