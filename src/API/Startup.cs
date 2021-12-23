namespace SkorinosGimnazija.API;

using Application;
using Filters;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using SchemaFilter;

public sealed class Startup
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
            app.UseSwaggerUI();
        }

        app.UseForwardedHeaders();
        app.UseCors();
        app.UseHealthChecks("/");

        app.UseRouting();

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
        services.AddApplication();
        services.AddInfrastructure(_config);

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        services.AddSwaggerGen(x =>
        {
            x.SupportNonNullableReferenceTypes();
            x.SchemaFilter<OpenApiImplicitRequiredSchemaFilter>();
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.SetPreflightMaxAge(TimeSpan.FromHours(1));
                x.WithOrigins(_config.GetSection("CORS:Origins").Get<string[]>());
            });
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownProxies.Clear();
            options.KnownNetworks.Clear();
            options.ForwardedHostHeaderName = "Host";
            options.ForwardedHeaders = ForwardedHeaders.All;
        });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddControllers(options => { options.Filters.Add<ApiExceptionFilter>(); })
            .AddFluentValidation(x => { x.AutomaticValidationEnabled = false; });
    }
}