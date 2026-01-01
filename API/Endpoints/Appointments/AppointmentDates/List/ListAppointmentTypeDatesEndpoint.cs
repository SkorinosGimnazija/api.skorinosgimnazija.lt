namespace API.Endpoints.Appointments.AppointmentDates.List;

public sealed class ListAppointmentTypeDatesEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, List<AppointmentDateResponse>, AppointmentDateMapper>
{
    public override void Configure()
    {
        Get("appointments/types/{id}/dates");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entities = await dbContext.AppointmentDates.AsNoTracking()
                           .Where(x => x.TypeId == req.Id)
                           .OrderBy(x => x.Date)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity).ToList(), ct);
    }
}