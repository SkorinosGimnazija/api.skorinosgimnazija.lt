namespace API.Endpoints.Appointments.AppointmentTypes.Create;

using API.Database.Entities.Authorization;

public sealed class CreateAppointmentTypeEndpoint(AppDbContext dbContext)
    : Endpoint<CreateAppointmentTypeRequest, AppointmentTypeDetailedResponse,
        AppointmentTypeDetailedMapper>
{
    public override void Configure()
    {
        Post("appointments/types");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateAppointmentTypeRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        entity.AdditionalInvitees = req.AdditionalInviteeIds
            .Select(id => dbContext.Attach(new AppUser { Id = id }).Entity)
            .ToList();

        entity.ExclusiveHosts = req.ExclusiveHostIds
            .Select(id => dbContext.Attach(new AppUser { Id = id }).Entity)
            .ToList();

        dbContext.AppointmentTypes.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}