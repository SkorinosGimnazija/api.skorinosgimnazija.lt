namespace API.Endpoints.Appointments.AppointmentDates.Update;

using API.Database.Entities.Appointments;

public sealed class UpdateAppointmentDatesEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateAppointmentDatesRequest, EmptyResponse>
{
    public override void Configure()
    {
        Post("appointments/dates");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateAppointmentDatesRequest req, CancellationToken ct)
    {
        var currentDates = await dbContext.AppointmentDates
                               .AsNoTracking()
                               .Where(x => x.TypeId == req.Id)
                               .ToDictionaryAsync(x => x.Date, ct);

        var datesToAdd = req.Dates
            .Where(x => !currentDates.ContainsKey(x))
            .Select(x => new AppointmentDate { Date = x, TypeId = req.Id });

        var datesToRemove = currentDates
            .Where(x => !req.Dates.Contains(x.Key))
            .Select(x => x.Value);

        dbContext.AppointmentDates.AddRange(datesToAdd);
        dbContext.AppointmentDates.RemoveRange(datesToRemove);

        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}