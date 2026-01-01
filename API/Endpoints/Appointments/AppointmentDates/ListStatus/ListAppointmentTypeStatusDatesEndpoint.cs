namespace API.Endpoints.Appointments.AppointmentDates.ListStatus;

public sealed class ListAppointmentTypeStatusDatesEndpoint(AppDbContext dbContext)
    : Endpoint<ListAppointmentTypeStatusDatesRequest, List<AppointmentDateStatusResponse>>
{
    public override void Configure()
    {
        Get("appointments/types/{typeId}/hosts/{hostId}/dates/status");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(
        ListAppointmentTypeStatusDatesRequest req, CancellationToken ct)
    {
        var dates = await dbContext.AppointmentDates.AsNoTracking()
                        .Where(x => x.TypeId == req.TypeId)
                        .Select(x => new AppointmentDateStatusResponse
                        {
                            Id = x.Id,
                            Date = x.Date,
                            IsRegistered = dbContext.Appointments.Any(a =>
                                a.AppointmentDateId == x.Id &&
                                a.HostId == req.HostId),
                            IsReserved = dbContext.AppointmentReservedDates.Any(r =>
                                r.DateId == x.Id &&
                                r.HostId == req.HostId)
                        })
                        .OrderBy(x => x.Date)
                        .ToListAsync(ct);

        await Send.OkAsync(dates, ct);
    }
}