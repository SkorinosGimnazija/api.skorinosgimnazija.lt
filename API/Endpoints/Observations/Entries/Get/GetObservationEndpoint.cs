namespace API.Endpoints.Observations.Entries.Get;

using API.Database.Entities.Observations;
using API.Extensions;

public sealed class GetObservationEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ObservationResponse, ObservationMapper>
{
    public override void Configure()
    {
        Get("observations/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Observations.AsNoTracking()
                         .Include(x => x.Lesson)
                         .Include(x => x.Student)
                         .Include(x => x.Creator)
                         .Include(x => x.Options)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (!HasAccess(entity))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }

    private bool HasAccess(Observation entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
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