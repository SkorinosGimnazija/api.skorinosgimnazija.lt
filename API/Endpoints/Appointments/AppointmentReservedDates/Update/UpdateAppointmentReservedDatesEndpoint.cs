namespace API.Endpoints.Appointments.AppointmentReservedDates.Update;

public sealed class UpdateAppointmentReservedDatesEndpoint(AppDbContext dbContext)
    : Endpoint<List<UpdateAppointmentReservedDatesRequest>,
        EmptyResponse,
        UpdateAppointmentReservedDatesMapper>
{
    public override void Configure()
    {
        Post("appointments/dates/reserved");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(
        List<UpdateAppointmentReservedDatesRequest> req, CancellationToken ct)
    {
        foreach (var group in req.GroupBy(x => x.HostId))
        {
            var hostId = group.Key;
            var newDates = group.ToDictionary(x => x.DateId);
            var dates = await dbContext.AppointmentReservedDates
                            .AsNoTracking()
                            .Where(x => x.HostId == hostId)
                            .ToDictionaryAsync(x => x.DateId, ct);

            var datesToAdd = newDates
                .Where(x => !dates.ContainsKey(x.Key))
                .Select(x => Map.ToEntity(x.Value));

            var datesToRemove = dates
                .Where(x => !newDates.ContainsKey(x.Key))
                .Select(x => x.Value);

            dbContext.AddRange(datesToAdd);
            dbContext.RemoveRange(datesToRemove);
        }

        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}