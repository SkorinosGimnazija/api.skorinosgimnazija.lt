namespace SkorinosGimnazija.Infrastructure.Identity;

using System.Security.Claims;
using Domain.Entities.Identity;
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
        identity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return identity;
    }
}