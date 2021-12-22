namespace SkorinosGimnazija.Infrastructure;

using System.Text;
using Application.Common.Interfaces;
using Domain.Entities.Identity;
using Extensions;
using FileManagement;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Identity;
using ImageOptimization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Options;
using Persistence;
using Search;
using Services;
using SkorinosGimnazija.Infrastructure.Calendar;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetNpgsqlConnectionString("DATABASE_URL"),
                x => { x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); });
        });

        services.AddIdentityCore<AppUser>()
            .AddRoles<AppUserRole>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddClaimsPrincipalFactory<AppUserClaimsPrincipal>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Secret"]))
                };
            });

        services.AddHttpClient();
        services.AddHttpContextAccessor();

        services.AddOptions<AlgoliaOptions>().BindConfiguration("Algolia");
        services.AddOptions<CloudinaryOptions>().BindConfiguration("Cloudinary");
        services.AddOptions<MediaManagerOptions>().BindConfiguration("FileManager");
        services.AddOptions<UrlOptions>().BindConfiguration("Urls");
        services.AddOptions<GoogleOptions>().BindConfiguration("Google");
        services.AddOptions<CalendarOptions>().BindConfiguration("Calendar");
        services.AddOptions<JwtOptions>().BindConfiguration("Jwt");

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<IImageOptimizer, CloudinaryImageOptimizer>();
        services.AddSingleton<ISearchClient, AlgoliaSearchClient>();
        services.AddSingleton<IMediaManager, MediaManager>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<ICalendarClient, GoogleCalendar>();
        services.AddSingleton<ITeachersService, GoogleTeachersService>();
        services.AddSingleton<TokenService>();

        return services;
    }
}