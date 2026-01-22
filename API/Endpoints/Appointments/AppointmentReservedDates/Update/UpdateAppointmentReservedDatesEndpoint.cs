namespace API.Endpoints.Appointments.AppointmentReservedDates.Update;

using API.Database.Entities.Appointments;

public sealed class UpdateAppointmentReservedDatesEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateAppointmentReservedDatesRequest, EmptyResponse>
{
    public override void Configure()
    {
        Post("appointments/dates/reserved");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(
        UpdateAppointmentReservedDatesRequest req, CancellationToken ct)
    {
        var currentDates = await dbContext.AppointmentReservedDates
                        .Where(x => x.HostId == req.Id)
                        .ToDictionaryAsync(x => x.DateId, ct);

        var datesToAdd = req.DateIds
            .Where(x => !currentDates.ContainsKey(x))
            .Select(x => new AppointmentReservedDate { DateId = x, HostId = req.Id });

        var datesToRemove = currentDates
            .Where(x => !req.DateIds.Contains(x.Key))
            .Select(x => x.Value);

        dbContext.AppointmentReservedDates.AddRange(datesToAdd);
        dbContext.AppointmentReservedDates.RemoveRange(datesToRemove);

        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}