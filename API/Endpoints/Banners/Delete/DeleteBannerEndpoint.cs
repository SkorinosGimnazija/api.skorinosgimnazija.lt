namespace API.Endpoints.Banners.Delete;

using API.Services.Storage;

public sealed class DeleteBannerEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("featured/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Banners.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        dbContext.Banners.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await new DeleteFileCommand { FileIds = [entity.ImageUrl] }.QueueJobAsync(ct: ct);

        await Send.NoContentAsync(ct);
    }
}