﻿namespace API;

using Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

internal static class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseKestrel(options => options.Limits.MaxRequestBodySize = null);
            });
    }

    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DataContext>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            await context.Database.MigrateAsync();

            await Seed.AddRoles(roleManager);
            await Seed.AddAdmin(userManager);
            await Seed.AddLanguages(context);
            await Seed.AddMenuLocations(context);
        }

        await host.RunAsync();
    }
}