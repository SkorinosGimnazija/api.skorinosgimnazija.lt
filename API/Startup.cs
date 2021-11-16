namespace API;

using Application.Core;
using Application.Core.Interfaces;
using Application.Posts;
using Core;
using Domain.Auth;
using Extensions;
using Filters;
using Infrastructure.Auth;
using Infrastructure.FileManager;
using Infrastructure.ImageOptimization;
using Infrastructure.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

internal sealed class Startup
{
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
        _config = config;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.Use(async (context, next) =>
        //{
        //    await Task.Delay(1000);
        //    await next.Invoke();
        //});

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseForwardedHeaders();

        app.UseCors();

        app.UseRouting();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            var controllers = endpoints.MapControllers();

            if (env.IsDevelopment())
            {
                controllers.WithMetadata(new AllowAnonymousAttribute());
            }
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<PublicUrls>(options =>
        {
            options.ApiUrl = _config.GetApiUrl();
            options.StaticUrl = _config.GetStaticUrl();
        });

        services.Configure<CloudinarySettings>(options =>
        {
            options.Url = _config.GetCloudinaryUrl();
        });

        services.Configure<FileManagerSettings>(options =>
        {
            options.UploadPath = _config.GetFileUploadPath();
        });

        services.Configure<AlgoliaSettings>(options =>
        {
            options.ApiKey = _config.GetAlgoliaApiKey();
            options.AppId = _config.GetAlgoliaAppId();
            options.IndexPrefix = _config.IsProduction() ? "prod_" : "dev_";
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownProxies.Clear();
            options.KnownNetworks.Clear();
            options.ForwardedHostHeaderName = "Host";
            options.ForwardedHeaders = ForwardedHeaders.All;
        });

        services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddSingleton<IImageOptimizer, CloudinaryImageOptimizer>();
        services.AddSingleton<ISearchClient, AlgoliaSearchClient>();
        services.AddSingleton<IFileManager, FileManager>();

        services.AddSwaggerGen();

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(_config.GetDatabaseConnectionString());
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(x =>
            {
                x.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                x.WithOrigins(_config.GetCorsOrigins());
            });
        });

        services.AddMediatR(typeof(PostCreate).Assembly);
        services.AddAutoMapper(typeof(PostCreate).Assembly);

        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DataContext>();

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = _config.GetGoogleClientId();
                options.ClientSecret = _config.GetGoogleClientSecret();
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.Secure = CookieSecurePolicy.Always;
            options.HttpOnly = HttpOnlyPolicy.Always;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToAccessDenied = x =>
            {
                x.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToLogin = x =>
            {
                x.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionLoggingFilter>();
        });
    }
}