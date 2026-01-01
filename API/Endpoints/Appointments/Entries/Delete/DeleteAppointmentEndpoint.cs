namespace API.Endpoints.Appointments.Entries.Delete;

using API.Database.Entities.Appointments;
using API.Extensions;

public sealed class DeleteAppointmentEndpoint(AppDbContext dbContext)
    : Endpoint<RouteGuidRequest>
{
    public override void Configure()
    {
        Delete("appointments/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteGuidRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Appointments.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (!await HasAccessAsync(entity))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        dbContext.Appointments.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await new CancelCalendarAppointmentCommand { AppointmentId = entity.Id }
            .QueueJobAsync(ct: ct);

        await Send.NoContentAsync(ct);
    }

    private async Task<bool> HasAccessAsync(Appointment entity)
    {
        // allow deleting only as attendee
        var canAccessRequested = await dbContext.Users
                                     .AnyAsync(x =>
                                         x.Id == User.GetId() &&
                                         x.Email == entity.AttendeeEmail);

        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}