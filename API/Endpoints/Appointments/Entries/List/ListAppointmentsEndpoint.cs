namespace API.Endpoints.Appointments.Entries.List;

using API.Extensions;

public sealed class ListAppointmentsEndpoint(AppDbContext dbContext)
    : Endpoint<ListAppointmentRequest,
        PaginatedListResponse<AppointmentResponse>,
        AppointmentMapper>
{
    public override void Configure()
    {
        Get("appointments");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListAppointmentRequest req, CancellationToken ct)
    {
        if (!HasAccess(req))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var query = dbContext.Appointments.AsNoTracking();

        if (req.UserId is not null)
        {
            var userId = req.UserId.Value;
            var userEmail = await dbContext.Users
                                .Where(x => x.Id == userId)
                                .Select(x => x.Email)
                                .FirstOrDefaultAsync(ct);

            query = query.Where(x => x.HostId == userId || x.AttendeeEmail == userEmail);
        }

        var entities = await query
                           .Include(x => x.Host)
                           .Include(x => x.AppointmentDate)
                           .Include(x => x.AppointmentDate.Type)
                           .OrderByDescending(x => x.AppointmentDate.Date)
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }

    private bool HasAccess(ListAppointmentRequest req)
    {
        var canAccessRequested = User.HasId(req.UserId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin() || User.IsManager();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}