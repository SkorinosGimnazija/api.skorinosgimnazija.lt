namespace API.Endpoints.Appointments.Entries.Public;

using API.Database.Entities.Appointments;
using API.Endpoints.Appointments.Entries.Create;
using EntityFramework.Exceptions.Common;
using Npgsql;

public sealed class CreateAppointmentPublicEndpoint(AppDbContext dbContext)
    : Endpoint<CreateAppointmentPublicRequest, AppointmentResponse, AppointmentMapper>
{
    public override void Configure()
    {
        Post("public/appointments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateAppointmentPublicRequest req, CancellationToken ct)
    {
        try
        {
            var appointment = Map.ToEntity(req);

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