namespace SkorinosGimnazija.Application.IntegrationTests;

using API;
using Domain.Entities.CMS;
using Domain.Entities.Identity;
using Infrastructure.Extensions;
using Infrastructure.ImageOptimization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mocks;
using Moq;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Xunit;

[CollectionDefinition("App", DisableParallelization = true)]
public class AppDefinition : ICollectionFixture<AppFixture>
{
}

public class AppFixture
{
    private readonly IConfigurationRoot _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public AppFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);
        var services = new ServiceCollection();

        startup.ConfigureServices(services);

        services.AddLogging();

        services.AddSingleton<IConfiguration>(_configuration);

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(x => x.EnvironmentName == "Production"));
        services.AddSingleton(Mock.Of<IHostEnvironment>(x => x.EnvironmentName == "Production"));
        services.AddSingleton(Mock.Of<IImageOptimizer>());

        SearchClientMock = new(services);
        CurrentUserMock = new(services);
        MediaManagerMock = new(services);
        CaptchaServiceMock = new(services);
        NotificationPublisherMock = new(services);
        CalendarServiceMock = new(services);
        EmployeeServiceMock = new(services);
        RevalidationServiceMock = new(services);

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        CreateDatabase();
    }

    public SearchClientMock SearchClientMock { get; }

    public PublisherMock NotificationPublisherMock { get; }

    public RevalidationServiceMock RevalidationServiceMock { get; }

    public CurrentUserMock CurrentUserMock { get; }

    public MediaManagerMock MediaManagerMock { get; }

    public CaptchaServiceMock CaptchaServiceMock { get; }

    public CalendarServiceMock CalendarServiceMock { get; }

    public EmployeeServiceMock EmployeeServiceMock { get; }

    private void CreateDatabase()
    {
        using var scope = _scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppUserRole>>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        Seed.AddRoles(roleManager).GetAwaiter().GetResult();
        Seed.AddLanguages(context).GetAwaiter().GetResult();
        Seed.AddMenuLocations(context).GetAwaiter().GetResult();
        Seed.AddAccomplishmentScales(context).GetAwaiter().GetResult();
        Seed.AddDays(context).GetAwaiter().GetResult();
        Seed.AddAccomplishmentAchievements(context).GetAwaiter().GetResult();
    }

    public void ResetData()
    {
        using var scope = _scopeFactory.CreateScope();
        var options = new RespawnerOptions
        {
            TablesToIgnore = new Table[]
            {
                "__EFMigrationsHistory",
                nameof(AppDbContext.Roles),
                nameof(AppDbContext.Languages),
                nameof(AppDbContext.MenuLocations),
                nameof(AppDbContext.Classdays),
                nameof(AppDbContext.AccomplishmentScales),
                nameof(AppDbContext.AccomplishmentAchievements)
            },
            DbAdapter = DbAdapter.Postgres
        };

        using var conn = new NpgsqlConnection(_configuration.GetNpgsqlConnectionString("DATABASE_URL"));
        conn.Open();

        var checkpoint = Respawner.CreateAsync(conn, options).GetAwaiter().GetResult();
        checkpoint.ResetAsync(conn).GetAwaiter().GetResult();

        CaptchaServiceMock.Reset();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(request);
    }

    public async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return (await context.FindAsync<TEntity>(keyValues))!;
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (entity is Post { LanguageId: 0 } post)
        {
            post.Language = new() { Slug = Path.GetRandomFileName(), Name = "Language" };
        }

        context.Add(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Set<TEntity>().CountAsync();
    }

    public async Task<AppUser> CreateUserAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var user = new AppUser { UserName = Guid.NewGuid().ToString(), DisplayName = Path.GetRandomFileName() };
        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new(result.Errors.First().Description);
        }

        return user;
    }
}