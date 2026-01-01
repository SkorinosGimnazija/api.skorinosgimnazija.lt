namespace API.Endpoints.Users.List;

using API.Extensions;

public sealed class ListTeachersEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<UserResponse>, UserMapper>
{
    public override void Configure()
    {
        Get("/teachers");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Users
                           .Teachers()
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}