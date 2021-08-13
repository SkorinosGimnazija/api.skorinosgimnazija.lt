namespace Persistence
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Auth;
    using Domain.CMS;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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

            foreach (var role in Roles.GetAllRoles())
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
            foreach (var role in Roles.GetAllRoles())
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }

                await roleManager.CreateAsync(new AppRole(role));
            }
        }

    }
}