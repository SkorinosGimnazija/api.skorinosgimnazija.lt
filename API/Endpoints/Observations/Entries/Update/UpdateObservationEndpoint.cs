namespace API.Endpoints.Observations.Entries.Update;

using API.Database.Entities.Observations;
using API.Extensions;

public sealed class UpdateObservationEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateObservationRequest, ObservationResponse, ObservationMapper>
{
    public override void Configure()
    {
        Put("observations");
        Roles(Auth.Role.Teacher, Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateObservationRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Observations
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

        Map.UpdateEntity(req, entity);
        entity.Options = await dbContext.ObservationOptions
                             .Where(x => req.OptionIds.Contains(x.Id))
                             .ToListAsync(ct);

        await dbContext.SaveChangesAsync(ct);

        entity = await dbContext.Observations.AsNoTracking()
                     .Include(x => x.Lesson)
                     .Include(x => x.Student)
                     .Include(x => x.Creator)
                     .Include(x => x.Options)
                     .FirstAsync(x => x.Id == entity.Id, ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }

    private bool HasAccess(Observation entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
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