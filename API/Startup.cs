namespace API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" }); });
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
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
            services.Configure<IdentityOptions>(
                options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.User.AllowedUserNameCharacters += "ąčęįšųūĄČĘĖĮŠŲŪ ";
                    });
            services.ConfigureApplicationCookie(
                options =>
                    {
                        options.Cookie.Name = "auth";
                        //   options.Cookie.Domain = _config.GetCookieDomain();

                        options.LoginPath = "/auth/login";
                        options.LogoutPath = "/auth/logout";
                        //  options.AccessDeniedPath = "";
                    });

            services.AddAuthentication()
                .AddGoogle(
                    options =>
                        {
                            options.ClientId = _config.GetGoogleClientId();
                            options.ClientSecret = _config.GetGoogleClientSecret();
                            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                        });
        }
    }
}