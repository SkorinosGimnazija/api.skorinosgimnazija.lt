namespace API.Endpoints.Appointments.AppointmentTypes.Get;

public sealed class GetAppointmentTypeEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, AppointmentTypeDetailedResponse, AppointmentTypeDetailedMapper>
{
    public override void Configure()
    {
        Get("appointments/types/{id}");
        Roles(Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.AppointmentTypes.AsNoTracking()
                         .Include(x => x.AdditionalInvitees)
                         .Include(x => x.ExclusiveHosts)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}