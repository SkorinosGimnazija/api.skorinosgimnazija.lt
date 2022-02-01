﻿namespace SkorinosGimnazija.Infrastructure.Persistence;

using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Appointments;
using Domain.Entities.Bullies;
using Domain.Entities.Identity;
using Domain.Entities.Teacher;
using Extensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

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

    public DbSet<BullyReport> BullyReports { get; set; } = default!;

    public DbSet<Menu> Menus { get; set; } = default!;

    public DbSet<Course> Courses { get; set; } = default!;

    public DbSet<MenuLocation> MenuLocations { get; set; } = default!;

    public DbSet<Post> Posts { get; set; } = default!;

    public DbSet<Appointment> Appointments { get; set; } = default!;

    public DbSet<AppointmentType> AppointmentTypes { get; set; } = default!;

    public DbSet<AppointmentDate> AppointmentDates { get; set; } = default!;

    public DbSet<AppointmentReservedDate> AppointmentReservedDates { get; set; } = default!;

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