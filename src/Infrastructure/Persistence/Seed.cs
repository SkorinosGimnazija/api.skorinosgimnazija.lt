namespace SkorinosGimnazija.Infrastructure.Persistence;

using Domain.Entities.Identity;
using Identity;
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

    public static async Task AddRoles(RoleManager<AppUserRole> roleManager)
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

    public static async Task AddLanguages(AppDbContext dbContext)
    {
        if (await dbContext.Languages.AnyAsync())
        {
            return;
        }

        dbContext.Languages.AddRange(
            new()
            {
                Name = "Lietuvių",
                Slug = "lt"
            },
            new()
            {
                Name = "Беларускі",
                Slug = "by"
            },
            new()
            {
                Name = "English",
                Slug = "en"
            }
        );

        await dbContext.SaveChangesAsync();
    }

    public static async Task AddMenuLocations(AppDbContext dbContext)
    {
        if (await dbContext.MenuLocations.AnyAsync())
        {
            return;
        }

        dbContext.MenuLocations.AddRange(
            new()
            {
                Name = "Header",
                Slug = "header"
            },
            new()
            {
                Name = "Side",
                Slug = "side"
            },
            new()
            {
                Name = "Footer",
                Slug = "footer"
            },
            new()
            {
                Name = "OFF",
                Slug = "off"
            }
        );

        await dbContext.SaveChangesAsync();
    }
}