namespace API.Endpoints.Banners.Get;

public sealed class GetBannerEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, BannerResponse, BannerMapper>
{
    public override void Configure()
    {
        Get("featured/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Banners.AsNoTracking()
                         .Include(x => x.Language)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}