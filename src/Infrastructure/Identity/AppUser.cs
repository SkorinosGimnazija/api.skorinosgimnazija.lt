namespace SkorinosGimnazija.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser<int>
{
    public string? DisplayName { get; set; }
}

public class AppUserRole : IdentityRole<int>
{
    public AppUserRole(string roleName) : base(roleName)
    {
    }

    public AppUserRole()
    {
    }
}