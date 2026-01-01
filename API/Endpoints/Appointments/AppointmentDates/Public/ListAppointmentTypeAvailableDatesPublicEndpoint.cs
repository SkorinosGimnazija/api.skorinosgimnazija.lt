namespace API.Endpoints.Appointments.AppointmentDates.Public;

using API.Endpoints.Appointments.AppointmentDates.ListAvailable;

public sealed class ListAppointmentTypeAvailableDatesPublicEndpoint(AppDbContext dbContext)
    : Endpoint<ListAppointmentTypeAvailableDatesRequest,
        IEnumerable<AppointmentDateResponse>,
        AppointmentDateMapper>
{
    public override void Configure()
    {
        Get("public/appointments/types/{typeId}/hosts/{hostId}/dates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        ListAppointmentTypeAvailableDatesRequest req, CancellationToken ct)
    {
        var type = await dbContext.AppointmentTypes.FindAsync([req.TypeId], ct);
        if (type is not { IsPublic: true })
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var entities = await dbContext.AppointmentDates.AsNoTracking()
                           .Where(x =>
                               x.Date > DateTime.UtcNow.AddHours(
                                   AppointmentSettings.AvailableDateOffsetInHours) &&
                               x.TypeId == req.TypeId &&
                               !dbContext.AppointmentReservedDates.Any(r =>
                                   r.DateId == x.Id &&
                                   r.HostId == req.HostId) &&
                               !dbContext.Appointments.Any(a =>
                                   a.AppointmentDateId == x.Id &&
                                   a.HostId == req.HostId))
                           .OrderBy(x => x.Date)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}