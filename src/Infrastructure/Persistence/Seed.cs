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
            await roleManager.CreateAsync(new(role!));
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
        //if (await dbContext.Classrooms.AnyAsync())
        //{
        //    return;
        //}

        //dbContext.Classrooms.AddRange(
        //    new() { Name = "1 klasė", Number = 1},
        //    new() { Name = "2 klasė", Number = 2 },
        //    new() { Name = "3 klasė", Number = 3 },
        //    new() { Name = "4 klasė", Number = 4 },
        //    new() { Name = "5 klasė", Number = 5 },
        //    new() { Name = "6 klasė", Number = 6 },
        //    new() { Name = "7 klasė", Number = 7 },
        //    new() { Name = "8 klasė", Number = 8 },
        //    new() { Name = "I g klasė", Number = 9 },
        //    new() { Name = "II g klasė", Number = 10 },
        //    new() { Name = "III g klasė", Number = 11 },
        //    new() { Name = "IV g klasė", Number = 12 }
        //);

        //await dbContext.SaveChangesAsync();
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