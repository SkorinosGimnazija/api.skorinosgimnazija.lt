namespace API.Endpoints.Posts.Create;

using API.Endpoints.Posts.Get;
using API.Services.Storage;

public sealed class CreatePostEndpoint(AppDbContext dbContext, FileManager fileManager)
    : Endpoint<CreatePostRequest, PostResponse, PostMapper>
{
    public override void Configure()
    {
        Post("posts");
        AllowFileUploads();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreatePostRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        if (req.NewFeaturedImage is not null)
        {
            entity.FeaturedImage =
                await fileManager.SaveImage(req.NewFeaturedImage, req.OptimizeImages);
        }

        if (req.NewImages is { Count: > 0 })
        {
            var savedFiles = await fileManager.SaveImages(req.NewImages, req.OptimizeImages);
            entity.Images = savedFiles.Values.ToList();
        }

        if (req.NewFiles is { Count: > 0 })
        {
            var savedFiles = await fileManager.SaveFiles(req.NewFiles);

            entity.Text = fileManager.ReplaceTextLinks(entity.Text, savedFiles);
            entity.IntroText = fileManager.ReplaceTextLinks(entity.IntroText, savedFiles);

            entity.Files = savedFiles.Values.ToList();
        }

        dbContext.Posts.Add(entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        await Send.CreatedAtAsync<GetPostEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}