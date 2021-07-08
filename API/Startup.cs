﻿namespace API
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
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
          
            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseCors();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<ForwardedHeadersOptions>(
                options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });
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
            services.Configure<CookiePolicyOptions>(options => { options.Secure = CookieSecurePolicy.Always; });
            services.ConfigureApplicationCookie(
                options =>
                    {
                        options.Cookie.Name = "auth";

                        options.LoginPath = "/auth/login";
                        options.LogoutPath = "/auth/logout";
                        //  options.AccessDeniedPath = "";
                         
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