namespace API.Endpoints.BullyReports.List;

using API.Extensions;

public sealed class ListBullyReportsEndpoint(AppDbContext dbContext)
    : Endpoint<ListBullyReportsRequest, PaginatedListResponse<BullyReportResponse>,
        BullyReportMapper>
{
    public override void Configure()
    {
        Get("bully-reports");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(ListBullyReportsRequest req, CancellationToken ct)
    {
        if (!HasAccess(req))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var query = dbContext.BullyReports.AsNoTracking()
            .Include(x => x.Creator)
            .AsQueryable();

        if (req.UserId is not null)
        {
            query = query.Where(x => x.CreatorId == req.UserId);
        }

        var entities = await query
                           .OrderByDescending(x => x.CreatedAt)
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }

    private bool HasAccess(ListBullyReportsRequest req)
    {
        var canAccessRequested = User.HasId(req.UserId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin() || User.IsSocialManager() || User.IsManager();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}