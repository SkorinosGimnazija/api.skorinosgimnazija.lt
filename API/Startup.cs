namespace API
{
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Posts;
    using Application.Utils;
    using Domain.Auth;
    using Extensions;
    using Filters;
    using FluentValidation.AspNetCore;
    using Infrastructure.Auth;
    using Infrastructure.FileManager;
    using Infrastructure.ImageOptimization;
    using Infrastructure.Search;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Persistence;

    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
            }

            app.UseForwardedHeaders();

            app.UseCors();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PublicUrls>(options =>
            {
                options.ApiUrl = _config.GetApiUrl();
                options.StaticUrl = _config.GetStaticUrl();
                options.WebUrl = _config.GetWebUrl();
            });
            services.Configure<CloudinarySettings>(options => { options.Url = _config.GetCloudinaryUrl(); });
            services.Configure<FileManagerSettings>(options => { options.UploadPath = _config.GetFileUploadPath(); });
            services.Configure<AlgoliaSettings>(options =>
            {
                options.ApiKey = _config.GetAlgoliaApiKey();
                options.AppId = _config.GetAlgoliaAppId();
            });

            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddSingleton<IImageOptimizer, CloudinaryImageOptimizer>();
            services.AddSingleton<ISearchClient, AlgoliaSearchClient>();
            services.AddSingleton<IFileManager, FileManager>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "Skorinos Gimnazija", Version = "v1" });
            });

            services.Configure<ForwardedHeadersOptions>(
                options =>
                {
                    options.KnownProxies.Clear();
                    options.KnownNetworks.Clear();
                    options.ForwardedHostHeaderName = "Host";
                    options.ForwardedHeaders = ForwardedHeaders.All;
                });

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(_config.GetDatabaseConnectionString());
            });

            services.AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        x =>
                        {
                            x.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                            x.WithOrigins(_config.GetCorsOrigins());
                        });
                });

            services.AddMediatR(typeof(PostCreate).Assembly);
            services.AddAutoMapper(typeof(PostCreate).Assembly);

            services.AddFluentValidation(
                options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<PostCreate>();
                    options.DisableDataAnnotationsValidation = true;
                });

            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters += "ąčęįšųūĄČĘĖĮŠŲŪ ";
                })
                .AddEntityFrameworkStores<DataContext>();

            services.Configure<CookiePolicyOptions>(options => { options.Secure = CookieSecurePolicy.Always; });

            services.ConfigureApplicationCookie(
                options =>
                {
                    options.Cookie.Name = "auth";

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

            services.AddAuthentication()
                .AddGoogle(
                    options =>
                    {
                        options.ClientId = _config.GetGoogleClientId();
                        options.ClientSecret = _config.GetGoogleClientSecret();
                    });

            services.AddControllers(options => { options.Filters.Add<ExceptionLoggingFilter>(); });
        }
    }
}