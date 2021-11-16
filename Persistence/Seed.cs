namespace Persistence;

using Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class Seed
{
    public static async Task AddAdmin(UserManager<AppUser> userManager)
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

    public static async Task AddRoles(RoleManager<AppRole> roleManager)
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

    public static async Task AddLanguages(DataContext dbContext)
    {
        if (await dbContext.Languages.AnyAsync())
        {
            return;
        }

        dbContext.Languages.AddRange(
            new() { Id = 1, Name = "Lietuvių", Slug = "lt" },
            new() { Id = 2, Name = "Беларускі", Slug = "by" },
            new() { Id = 3, Name = "English", Slug = "en" }
        );

        await dbContext.SaveChangesAsync();
    }

    public static async Task AddMenuLocations(DataContext dbContext)
    {
        if (await dbContext.MenuLocations.AnyAsync())
        {
            return;
        }

        dbContext.MenuLocations.AddRange(
            new() { Id = 1, Name = "Header", Slug = "header" },
            new() { Id = 2, Name = "Side", Slug = "side" },
            new() { Id = 3, Name = "Footer", Slug = "footer" }
        );

        await dbContext.SaveChangesAsync();
    }
}