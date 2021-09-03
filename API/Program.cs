global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Logging;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Linq;
global using System;

using API.Extensions;
using API.Filters;
using Application.Core.MappingProfiles;
using Application.Interfaces;
using Application.Posts;
using Domain.Auth;
using FluentValidation.AspNetCore;
using Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Persistence;
using API.Utils;
using Microsoft.AspNetCore.Builder;

// TODO clean this up ?..

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddScoped<IUserAccessor, UserAccessor>();
//services.AddTransient<ExceptionLoggingFilter>();

services.AddControllers(options =>
{
   // options.Filters.Add(typeof(ExceptionLoggingFilter));
    options.Filters.Add<ExceptionLoggingFilter>(); 
});

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Skorinos Gimnazija", Version = "v1" });
});

services.Configure<ForwardedHeadersOptions>(
    options =>
    {
        options.KnownProxies.Clear();
        options.KnownNetworks.Clear();
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(config.GetDatabaseConnectionString());
});

services.AddFluentValidation(
    options =>
    {
       // options.RegisterValidatorsFromAssemblyContaining<PostCreate>();
        options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        options.DisableDataAnnotationsValidation = true;
    });

services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            x =>
            {
                x.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                x.WithOrigins(config.GetCorsOrigins());
            });
    });

services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
//services.AddMediatR(typeof(PostProfiles).Assembly);
//services.AddAutoMapper(typeof(PostProfiles).Assembly);
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
            options.ClientId = config.GetGoogleClientId();
            options.ClientSecret = config.GetGoogleClientSecret();
        });




var app = builder.Build();




using (var scope = app.Services.CreateScope())
{
    var appServices = scope.ServiceProvider;

    var context = appServices.GetRequiredService<DataContext>();
    var roleManager = appServices.GetRequiredService<RoleManager<AppRole>>();
    var userManager = appServices.GetRequiredService<UserManager<AppUser>>();

    await context.Database.MigrateAsync();

    await Seed.CreateRoles(roleManager);
    await Seed.CreateAdmin(userManager);
}






if (builder.Environment.IsDevelopment())
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

app.MapControllers();

app.Run();
