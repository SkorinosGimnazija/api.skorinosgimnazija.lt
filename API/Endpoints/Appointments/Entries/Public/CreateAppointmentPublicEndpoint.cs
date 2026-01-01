namespace API.Endpoints.Appointments.Entries.Public;

using API.Endpoints.Appointments.Entries.Create;

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

        await Send.OkAsync(Map.FromEntity(appointment), ct);
    }
}