namespace SkorinosGimnazija.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

public class AppUserRole : IdentityRole<int>
{
    public AppUserRole(string roleName) : base(roleName)
    {
    }

    public AppUserRole()
    {
    }
}