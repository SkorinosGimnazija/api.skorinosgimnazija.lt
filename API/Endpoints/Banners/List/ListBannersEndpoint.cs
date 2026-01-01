namespace API.Endpoints.Banners.List;

public sealed class ListBannersEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<BannerResponse>, BannerMapper>
{
    public override void Configure()
    {
        Get("featured");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Banners.AsNoTracking()
                           .Include(x => x.Language)
                           .OrderBy(x => x.Order)
                           .ThenBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}