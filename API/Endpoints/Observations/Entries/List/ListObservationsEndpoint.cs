namespace API.Endpoints.Observations.Entries.List;

using API.Extensions;

public sealed class ListObservationsEndpoint(AppDbContext dbContext)
    : Endpoint<ListObservationsRequest,
        PaginatedListResponse<ObservationResponse>,
        ObservationMapper>
{
    public override void Configure()
    {
        Get("observations");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListObservationsRequest req, CancellationToken ct)
    {
        if (!HasAccess(req))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var query = dbContext.Observations.AsNoTracking()
            .Include(x => x.Lesson)
            .Include(x => x.Student)
            .Include(x => x.Creator)
            .Include(x => x.Options)
            .AsQueryable();

        if (req.CreatorId is not null)
        {
            query = query
                .Where(x => x.CreatorId == req.CreatorId)
                .OrderByDescending(x => x.Id);
        }
        else
        {
            query = query
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id);
        }

        var entities = await query
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }

    private bool HasAccess(ListObservationsRequest req)
    {
        var canAccessRequested = User.HasId(req.CreatorId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin() || User.IsManager() || User.IsSocialManager();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}