namespace Persistence;

using Domain.Auth;
using Microsoft.AspNetCore.Identity;

public static class Seed
{
    public static async Task CreateAdmin(UserManager<AppUser> userManager)
    {
        const string AdminEmail = "admin@skorinosgimnazija.lt";
        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin == null)
        {
            return;
        }

        foreach (var role in Auth.Roles)
        {
            if (await userManager.IsInRoleAsync(admin, role))
            {
                continue;
            }

            await userManager.AddToRoleAsync(admin, role);
        }
    }

    public static async Task CreateRoles(RoleManager<AppRole> roleManager)
    {
        foreach (var role in Auth.Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            await roleManager.CreateAsync(new(role));
        }
    }
}