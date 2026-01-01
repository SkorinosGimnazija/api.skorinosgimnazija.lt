namespace API.Endpoints.Appointments.AppointmentTypes.Update;

public sealed class UpdateAppointmentTypeEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateAppointmentTypeRequest, AppointmentTypeDetailedResponse,
        AppointmentTypeDetailedMapper>
{
    public override void Configure()
    {
        Put("appointments/types");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateAppointmentTypeRequest req, CancellationToken ct)
    {
        var entity = await dbContext.AppointmentTypes
                         .Include(x => x.AdditionalInvitees)
                         .Include(x => x.ExclusiveHosts)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);

        entity.AdditionalInvitees = await dbContext.Users
                                        .Where(x => req.AdditionalInviteeIds.Contains(x.Id))
                                        .ToListAsync(ct);

        entity.ExclusiveHosts = await dbContext.Users
                                    .Where(x => req.ExclusiveHostIds.Contains(x.Id))
                                    .ToListAsync(ct);

        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}