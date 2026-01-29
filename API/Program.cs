global using Microsoft.EntityFrameworkCore;
global using FastEndpoints;
global using FluentValidation;
global using API.Services.Identity;
global using API.Database;
global using Microsoft.Extensions.Logging;
using API.Exceptions;
using API.Extensions;
using API.Services.Background;
using API.Services.Calendar;
using API.Services.Captcha;
using API.Services.Email;
using API.Services.Employee;
using API.Services.ImageOptimization;
using API.Services.JobQueue;
using API.Services.Options;
using API.Services.Settings;
using API.Services.Storage;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

if (builder.Environment.IsProduction())
{
    builder.WebHost.UseSentry(o =>
    {
        o.EnableLogs = true;
        o.SampleRate = 1.0f;
        o.TracesSampleRate = 0.35f;
        o.Dsn = config["Sentry:Dsn"]!;
        o.AddExceptionFilter(new SentryIgnoredExceptions());
    });
}

services.Configure<KestrelServerOptions>(o => o.Limits.MaxRequestBodySize = null);
services.Configure<JwtCreationOptions>(o =>
{
    o.SigningKey = config["Jwt:Secret"]!;
    o.SigningAlgorithm = SecurityAlgorithms.HmacSha256;
});
services.AddAuthenticationJwtBearer(o => o.SigningKey = config["Jwt:Secret"]!);
services.AddAuthorization();
services.AddProblemDetails();
services.AddExceptionHandler<ConstraintExceptionHandler>();
services.AddFastEndpoints();
services.AddResponseCaching();
services.AddJobQueues<JobRecord, JobStorageProvider>();
services.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
    o.DocumentSettings = d => d.MarkNonNullablePropsAsRequired();
});
services.AddHttpClient();
services.AddMemoryCache();
services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseNpgsql(config.GetNpgsqlConnectionString("DATABASE_URL"), o =>
    {
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        o.ConfigureDataSource(ds => ds.EnableDynamicJson());
    });
});

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
        policy.SetPreflightMaxAge(TimeSpan.FromHours(2));
        policy.WithOrigins(config.GetSection("CORS:Origins").Get<string[]>()!);
    });
});
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Clear();
    options.KnownIPNetworks.Clear();
    options.ForwardedHostHeaderName = "Host";
    options.ForwardedHeaders = ForwardedHeaders.All;
});

services.AddOptionsAndValidate<GoogleOptions>("Google");
services.AddOptionsAndValidate<UrlOptions>("Urls");
services.AddOptionsAndValidate<CloudinaryOptions>("Cloudinary");
services.AddOptionsAndValidate<CalendarOptions>("Calendar");
services.AddOptionsAndValidate<GroupOptions>("Groups");
services.AddOptionsAndValidate<CaptchaOptions>("Captcha");
services.AddOptionsAndValidate<EmailOptions>("Email");
services.AddOptionsAndValidate<GoogleDriveOptions>("GoogleDrive");
services.AddOptionsAndValidate<PostRevalidationOptions>("PostRevalidation");
services.AddOptionsAndValidate<NotificationOptions>("Notifications");
services.AddOptionsAndValidate<JwtOptions>("Jwt");

services.AddScoped<SettingsProvider>();
services.AddScoped<IdentityService>();
services.AddSingleton(TimeProvider.System);
services.AddSingleton<EmployeeService>();
services.AddSingleton<FileManager>();
services.AddSingleton<CaptchaService>();
services.AddSingleton<IImageOptimizer, CloudinaryImageOptimizer>();
services.AddSingleton<IStorageService, GoogleDriveService>();

if (builder.Environment.IsProduction())
{
    services.AddHostedService<RecentFilesCacheService>();
    services.AddHostedService<UserSyncService>();
    services.AddSingleton<ICalendarService, GoogleCalendarService>();
    services.AddSingleton<IEmailService, GoogleEmailService>();
}
else
{
    services.AddSingleton<ICalendarService, DevCalendarService>();
    services.AddSingleton<IEmailService, DevEmailService>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.UseExceptionHandler();
app.UseForwardedHeaders();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/");
app.UseResponseCaching();
app.UseFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
    c.Validation.EnableDataAnnotationsSupport = true;
    c.Endpoints.NameGenerator = ctx => ctx.EndpointType.Name.TrimEnd("Endpoint");
});
app.UseJobQueues(o =>
{
    o.MaxConcurrency = 1;
    o.StorageProbeDelay = TimeSpan.FromMinutes(5);
});

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();