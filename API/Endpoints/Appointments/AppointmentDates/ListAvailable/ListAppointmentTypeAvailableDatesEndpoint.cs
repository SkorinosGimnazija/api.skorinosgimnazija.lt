namespace API.Endpoints.Appointments.AppointmentDates.ListAvailable;

public sealed class ListAppointmentTypeAvailableDatesEndpoint(AppDbContext dbContext)
    : Endpoint<ListAppointmentTypeAvailableDatesRequest,
        IEnumerable<AppointmentDateResponse>,
        AppointmentDateMapper>
{
    public override void Configure()
    {
        Get("appointments/types/{typeId}/hosts/{hostId}/dates");
        Roles(Auth.Role.Admin, Auth.Role.Manager, Auth.Role.Teacher);
    }

    public override async Task HandleAsync(
        ListAppointmentTypeAvailableDatesRequest req, CancellationToken ct)
    {
        var entities = await dbContext.AppointmentDates.AsNoTracking()
                           .Where(x =>
                               x.TypeId == req.TypeId &&
                               x.Date > DateTime.UtcNow.AddHours(
                                   AppointmentSettings.AvailableDateOffsetInHours) &&
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