namespace API.Endpoints.School.Classtimes.Delete;

public sealed class DeleteClasstimeEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("school/classtimes/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Classtimes.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        dbContext.Classtimes.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}