namespace API.Endpoints.Appointments.AppointmentDates.Update;

public sealed class UpdateAppointmentDatesEndpoint(AppDbContext dbContext)
    : Endpoint<List<UpdateAppointmentDatesRequest>, EmptyResponse, AppointmentDateMapper>
{
    public override void Configure()
    {
        Post("appointments/dates");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(
        List<UpdateAppointmentDatesRequest> req, CancellationToken ct)
    {
        foreach (var group in req.GroupBy(x => x.TypeId))
        {
            var typeId = group.Key;
            var newDates = group.ToDictionary(x => x.Date);
            var dates = await dbContext.AppointmentDates
                            .AsNoTracking()
                            .Where(x => x.TypeId == typeId)
                            .ToDictionaryAsync(x => x.Date, ct);

            var datesToAdd = newDates
                .Where(x => !dates.ContainsKey(x.Key))
                .Select(x => Map.ToEntity(x.Value));

            var datesToRemove = dates
                .Where(x => !newDates.ContainsKey(x.Key))
                .Select(x => x.Value);

            dbContext.AppointmentDates.AddRange(datesToAdd);
            dbContext.AppointmentDates.RemoveRange(datesToRemove);
        }

        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}