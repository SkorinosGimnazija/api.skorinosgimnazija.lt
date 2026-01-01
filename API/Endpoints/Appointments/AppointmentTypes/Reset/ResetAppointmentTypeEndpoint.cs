namespace API.Endpoints.Appointments.AppointmentTypes.Reset;

public sealed class ResetAppointmentTypeEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Post("appointments/type/{id}/reset");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        await dbContext.Appointments
            .Where(x => x.AppointmentDate.TypeId == req.Id)
            .ExecuteDeleteAsync(ct);

        await dbContext.AppointmentDates
            .Where(x => x.TypeId == req.Id)
            .ExecuteDeleteAsync(ct);

        await Send.NoContentAsync(ct);
    }
}