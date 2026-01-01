namespace API.Endpoints.Posts.Delete;

using API.Services.Storage;

public sealed class DeletePostEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("posts/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Posts.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var files = new List<string>();

        if (entity.Files is not null)
        {
            files.AddRange(entity.Files);
        }

        if (entity.Images is not null)
        {
            files.AddRange(entity.Images);
        }

        if (entity.FeaturedImage is not null)
        {
            files.Add(entity.FeaturedImage);
        }

        dbContext.Posts.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        if (files.Count > 0)
        {
            await new DeleteFileCommand { FileIds = files }.QueueJobAsync(ct: ct);
        }

        await Send.NoContentAsync(ct);
    }
}