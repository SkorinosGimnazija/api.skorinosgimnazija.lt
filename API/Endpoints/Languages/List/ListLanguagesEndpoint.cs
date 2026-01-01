namespace API.Endpoints.Languages.List;

public sealed class ListLanguagesEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<LanguageResponse>, LanguageMapper>
{
    public override void Configure()
    {
        Get("/languages");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Languages
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}