namespace Persistence
{
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.AspNetCore.Identity;

    public static class Seed
    {
        private const string AdminEmail = "admin@skorinosgimnazija.lt";

        public static async Task CreateAdmin(UserManager<AppUser> userManager)
        {
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