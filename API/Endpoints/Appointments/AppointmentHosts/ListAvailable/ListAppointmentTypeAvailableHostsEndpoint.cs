namespace API.Endpoints.Appointments.AppointmentHosts.ListAvailable;

using API.Extensions;

public sealed class ListAppointmentTypeAvailableHostsEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest,
        IEnumerable<AppointmentHostResponse>,
        ListAppointmentTypeAvailableHostsMapper>
{
    public override void Configure()
    {
        Get("appointments/types/{id}/hosts");
        Roles(Auth.Role.Teacher, Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var exclusiveHosts = await dbContext.AppointmentTypes.AsNoTracking()
                                 .Where(x => x.Id == req.Id)
                                 .SelectMany(x => x.ExclusiveHosts)
                                 .OrderBy(x => x.Name)
                                 .ToListAsync(ct);

        var entities = exclusiveHosts.Count > 0
                           ? exclusiveHosts
                           : await dbContext.Users.Teachers().ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}