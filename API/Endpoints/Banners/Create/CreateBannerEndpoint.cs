namespace API.Endpoints.Banners.Create;

using API.Endpoints.Banners.Get;
using API.Services.Storage;
using SixLabors.ImageSharp;

public sealed class CreateBannerEndpoint(AppDbContext dbContext, FileManager fileManager)
    : Endpoint<CreateBannerRequest, BannerResponse, BannerMapper>
{
    public override void Configure()
    {
        Post("featured");
        AllowFileUploads();
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateBannerRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        await using var imageStream = req.Image.OpenReadStream();
        using var imageInfo = await Image.LoadAsync(imageStream, ct);

        entity.Height = imageInfo.Height;
        entity.Width = imageInfo.Width;
        entity.ImageUrl = await fileManager.SaveFile(req.Image);

        dbContext.Banners.Add(entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Language).LoadAsync(ct);

        await Send.CreatedAtAsync<GetBannerEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}