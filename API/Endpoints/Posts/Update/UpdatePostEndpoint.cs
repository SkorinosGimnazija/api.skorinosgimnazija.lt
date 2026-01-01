namespace API.Endpoints.Posts.Update;

using API.Services.Storage;

public sealed class UpdatePostEndpoint(AppDbContext dbContext, FileManager fileManager)
    : Endpoint<UpdatePostRequest, PostResponse, PostMapper>
{
    public override void Configure()
    {
        Put("posts");
        AllowFileUploads();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdatePostRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Posts.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);

        var deleteIds = new List<string>();

        if ((req.OldFeaturedImage is null || req.NewFeaturedImage is not null) &&
            entity.FeaturedImage is not null)
        {
            deleteIds.Add(entity.FeaturedImage);
            entity.FeaturedImage = null;
        }

        if (req.NewFeaturedImage is not null)
        {
            entity.FeaturedImage =
                await fileManager.SaveImage(req.NewFeaturedImage, req.OptimizeImages);
        }

        var existingImages = entity.Images ?? [];
        var keepImages = req.OldImages ?? [];
        deleteIds.AddRange(existingImages.Except(keepImages));

        if (req.NewImages is { Count: > 0 })
        {
            var savedFiles = await fileManager.SaveImages(req.NewImages, req.OptimizeImages);
            keepImages.AddRange(savedFiles.Values);
        }

        entity.Images = keepImages.Count > 0 ? keepImages : null;

        var existingFiles = entity.Files ?? [];
        var keepFiles = fileManager.GetLinkedFiles(entity.Text + entity.IntroText, existingFiles);
        deleteIds.AddRange(existingFiles.Except(keepFiles));

        if (req.NewFiles is { Count: > 0 })
        {
            var savedFiles = await fileManager.SaveFiles(req.NewFiles);

            entity.Text = fileManager.ReplaceTextLinks(entity.Text, savedFiles);
            entity.IntroText = fileManager.ReplaceTextLinks(entity.IntroText, savedFiles);

            keepFiles.AddRange(savedFiles.Values);
        }

        entity.Files = keepFiles.Count > 0 ? keepFiles : null;

        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        if (deleteIds.Count > 0)
        {
            await new DeleteFileCommand { FileIds = deleteIds }.QueueJobAsync(ct: ct);
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}