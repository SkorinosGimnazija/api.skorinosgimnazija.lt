namespace API.Endpoints.Users.List;

public sealed class ListUsersEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<UserResponse>, UserMapper>
{
    public override void Configure()
    {
        Get("/users");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Users
                           .AsNoTracking()
                           .OrderBy(x => x.Name)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}