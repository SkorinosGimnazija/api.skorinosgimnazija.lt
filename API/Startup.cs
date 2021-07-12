namespace API
{
    using System.Threading.Tasks;
    using Application.Core;
    using Domain;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Persistence;
    using Utils;

    public class Startup
    {
        private readonly ConfigUtils _config;

        public Startup(IConfiguration configuration)
        {
            _config = new ConfigUtils(configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            services.AddControllers();

            services.Configure<ForwardedHeadersOptions>(
                options =>
                    {
                        options.KnownProxies.Clear();
                        options.KnownNetworks.Clear();
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                    });

            services.AddDbContext<DataContext>(options => { options.UseNpgsql(_config.GetDatabaseConnectionString()); });

            services.AddCors(
                options =>
                    {
                        options.AddDefaultPolicy(
                            x =>
                                {
                                    x.AllowAnyMethod();
                                    x.AllowAnyHeader();
                                    x.AllowCredentials();
                                    x.WithOrigins(_config.GetCorsOrigins());
                                });
                    });

            services.AddMediatR(typeof(MappingProfiles).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DataContext>();

            services.Configure<IdentityOptions>(
                options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.User.AllowedUserNameCharacters += "ąčęįšųūĄČĘĖĮŠŲŪ ";
                    });

            services.Configure<CookiePolicyOptions>(options => { options.Secure = CookieSecurePolicy.Always; });

            services.ConfigureApplicationCookie(
                options =>
                    {
                        options.Cookie.Name = "auth";

                        options.LoginPath = "/auth/login";
                        options.LogoutPath = "/auth/logout";

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
        }
    }
}