namespace Persistence
{

using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

public static class Seed
{
    private const string AdminEmail = "admin@skorinosgimnazija.lt";

    public static async Task CreateAdmin(UserManager<IdentityUser> userManager)
    {
        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin == null)
        {
            return;
        }

        if (await userManager.IsInRoleAsync(admin, Roles.Admin))
        {
            return;
        }

        await userManager.AddToRoleAsync(admin, Roles.Admin);
    }

    public static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles.GetAllRoles())
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

}