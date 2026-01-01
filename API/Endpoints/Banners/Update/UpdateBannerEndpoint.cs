namespace API.Endpoints.Banners.Update;

using API.Services.Storage;

public sealed class UpdateBannerEndpoint(AppDbContext dbContext, FileManager fileManager)
    : Endpoint<UpdateBannerRequest, BannerResponse, BannerMapper>
{
    public override void Configure()
    {
        Put("featured");
        AllowFileUploads();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateBannerRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Banners.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);

        if (req.Image is not null)
        {
            await new DeleteFileCommand { FileIds = [entity.ImageUrl] }.QueueJobAsync(ct: ct);

            (entity.Width, entity.Height) = fileManager.GetImageDimensions(req.Image);
            entity.ImageUrl = await fileManager.SaveFile(req.Image);
        }

        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}