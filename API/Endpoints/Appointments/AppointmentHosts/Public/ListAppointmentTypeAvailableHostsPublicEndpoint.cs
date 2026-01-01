namespace API.Endpoints.Appointments.AppointmentHosts.Public;

using API.Endpoints.Appointments.AppointmentHosts.ListAvailable;
using API.Extensions;

public sealed class ListAppointmentTypeAvailableHostsPublicEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest,
        IEnumerable<AppointmentHostResponse>,
        ListAppointmentTypeAvailableHostsMapper>
{
    public override void Configure()
    {
        Get("public/appointments/types/{id}/hosts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var type = await dbContext.AppointmentTypes.FindAsync([req.Id], ct);
        if (type is not { IsPublic: true })
        {
            await Send.NotFoundAsync(ct);
            return;
        }

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