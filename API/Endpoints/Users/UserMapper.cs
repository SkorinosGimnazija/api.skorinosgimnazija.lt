namespace API.Endpoints.Users;

using API.Database.Entities.Authorization;

public sealed class UserMapper : ResponseMapper<UserResponse, AppUser>
{
    public override UserResponse FromEntity(AppUser e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            NormalizedName = e.NormalizedName
        };
    }
}