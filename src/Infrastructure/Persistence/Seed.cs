namespace SkorinosGimnazija.Infrastructure.Persistence;

using Domain.Entities.Identity;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class Seed
{
    public static async Task AddRoles(RoleManager<AppUserRole> roleManager)
    {
        var currentRoles = await roleManager.Roles.Select(x => x.Name).ToListAsync();

        foreach (var role in Auth.AllRoles.Except(currentRoles))
        {
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

    public static async Task AddAccomplishmentScales(AppDbContext dbContext)
    {
        if (await dbContext.AccomplishmentScales.AnyAsync())
        {
            return;
        }

        dbContext.AccomplishmentScales.AddRange(
            new() { Name = "Gimnazijos" },
            new() { Name = "Miesto" },
            new() { Name = "Regioninis" },
            new() { Name = "Respublikinis" },
            new() { Name = "Tarptautinis" }
        );

        await dbContext.SaveChangesAsync();
    }

    public static async Task AddAccomplishmentClassrooms(AppDbContext dbContext)
    {
        if (await dbContext.AccomplishmentClassrooms.AnyAsync())
        {
            return;
        }

        dbContext.AccomplishmentClassrooms.AddRange(
            new() { Name = "1 klasė" },
            new() { Name = "2 klasė" },
            new() { Name = "3 klasė" },
            new() { Name = "4 klasė" },
            new() { Name = "5 klasė" },
            new() { Name = "6 klasė" },
            new() { Name = "7 klasė" },
            new() { Name = "8 klasė" },
            new() { Name = "I g klasė" },
            new() { Name = "II g klasė" },
            new() { Name = "III g klasė" },
            new() { Name = "IV g klasė" }
        );

        await dbContext.SaveChangesAsync();
    }

    public static async Task AddAccomplishmentAchievements(AppDbContext dbContext)
    {
        if (await dbContext.AccomplishmentAchievements.AnyAsync())
        {
            return;
        }

        dbContext.AccomplishmentAchievements.AddRange(
            new() { Name = "Dalyvavimas" },
            new() { Name = "Padėka" },
            new() { Name = "Laureatas" },
            new() { Name = "I vieta" },
            new() { Name = "II vieta" },
            new() { Name = "III vieta" }
        );

        await dbContext.SaveChangesAsync();
    }
}