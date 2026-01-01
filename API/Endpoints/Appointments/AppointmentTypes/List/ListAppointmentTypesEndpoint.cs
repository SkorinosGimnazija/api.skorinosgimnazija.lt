namespace API.Endpoints.Appointments.AppointmentTypes.List;

public sealed class ListAppointmentTypesEndpoint(AppDbContext dbContext)
    : Endpoint<ListAppointmentTypesRequest, IEnumerable<AppointmentTypeDetailedResponse>,
        AppointmentTypeDetailedMapper>
{
    public override void Configure()
    {
        Get("appointments/types");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListAppointmentTypesRequest req, CancellationToken ct)
    {
        var query = dbContext.AppointmentTypes.AsNoTracking();

        if (req.ShowPrivateOnly is true)
        {
            query = query.Where(x => !x.IsPublic);
        }

        var entities = await query
                           .OrderByDescending(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}