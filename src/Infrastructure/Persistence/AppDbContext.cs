namespace SkorinosGimnazija.Infrastructure.Persistence;

using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Identity;
using Extensions;
using FluentValidation.Results;
using Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using SkorinosGimnazija.Domain.Entities.Teacher;

public sealed class AppDbContext : IdentityDbContext<AppUser, AppUserRole, int>, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException ie && ie.IsConstraintViolation())
        {
            var validationFailure = ie.ConstraintName switch
            {
                //"ConstraintName" => new ("PropertyName", "Description"),
                _ => new ValidationFailure(ie.ConstraintName ?? ie.ColumnName, "Constraint violation")
            };

            throw new ValidationException(validationFailure);
        }
    }

    public DbSet<Language> Languages { get; set; } = default!;

    public DbSet<Banner> Banners { get; set; } = default!;

    public DbSet<Menu> Menus { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;

    public DbSet<MenuLocation> MenuLocations { get; set; } = default!;

    public DbSet<Post> Posts { get; set; } = default!;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}