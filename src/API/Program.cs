namespace SkorinosGimnazija.API;

using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

public static class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog((context, _, configuration) =>
            {
                configuration
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .WriteTo.Console();

                if (context.HostingEnvironment.IsProduction())
                {
                    configuration
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                        .WriteTo.MicrosoftTeams(
                            context.Configuration["Teams:WebHook"],
                            restrictedToMinimumLevel: LogEventLevel.Warning
                        );
                }
            })
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
            var context = services.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            var roleManager = services.GetRequiredService<RoleManager<AppUserRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            await Seed.AddRoles(roleManager);
            await Seed.AddAdmin(userManager);
            await Seed.AddLanguages(context);
            await Seed.AddMenuLocations(context);
        }

        await host.RunAsync();
    }
}