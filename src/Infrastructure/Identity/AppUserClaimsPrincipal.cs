namespace SkorinosGimnazija.Infrastructure.Identity;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public sealed class AppUserClaimsPrincipal : UserClaimsPrincipalFactory<AppUser, AppUserRole>
{
    public AppUserClaimsPrincipal(
        UserManager<AppUser> userManager,
        RoleManager<AppUserRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = new ClaimsIdentity();
        var roles = await UserManager.GetRolesAsync(user);

        identity.AddClaim(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
        identity.AddClaim(new(ClaimTypes.Name, user.DisplayName ?? user.Email));
        identity.AddClaim(new(ClaimTypes.Email, user.Email));
        identity.AddClaims(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        return identity;
    }
}