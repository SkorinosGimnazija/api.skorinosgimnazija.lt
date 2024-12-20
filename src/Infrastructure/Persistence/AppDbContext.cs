﻿namespace SkorinosGimnazija.Infrastructure.Persistence;

using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Accomplishments;
using Domain.Entities.Appointments;
using Domain.Entities.Bullies;
using Domain.Entities.CMS;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Domain.Entities.Observation;
using Domain.Entities.School;
using Domain.Entities.TechReports;
using Domain.Entities.Timetable;
using Extensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

public sealed class AppDbContext(DbContextOptions options)
    : IdentityDbContext<AppUser, AppUserRole, int>(options), IAppDbContext
{
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
                "IX_Appointments_UserName_AttendeeEmail" => new("Klaida",
                    "Jūs jau esate užsiregistravęs (-usi) pas pasirinktą mokytoją"),
                "IX_Appointments_DateId_UserName" => new("Klaida", "Pasirinktas laikas užimtas"),
                "IX_Appointments_DateId_AttendeeEmail" => new("Klaida",
                    "Pasirinktu laiku jūs jau esate užsiregistravęs (-usi)"),
                "IX_Timetable_DayId_RoomId_TimeId" => new("Klaida", "Pasirinktu laiku klasė jau turi pamoką"),
                _ => new ValidationFailure(ie.ConstraintName ?? ie.ColumnName, "Constraint violation")
            };

            throw new ValidationException(validationFailure);
        }
    }

    public DbSet<Language> Languages { get; set; } = default!;

    public DbSet<Banner> Banners { get; set; } = default!;

    public DbSet<BullyReport> BullyReports { get; set; } = default!;

    public DbSet<BullyJournalReport> BullyJournalReports { get; set; } = default!;

    public DbSet<TechJournalReport> TechJournalReports { get; set; } = default!;

    public DbSet<Menu> Menus { get; set; } = default!;

    public DbSet<Course> Courses { get; set; } = default!;

    public DbSet<MenuLocation> MenuLocations { get; set; } = default!;

    public DbSet<Post> Posts { get; set; } = default!;

    public DbSet<Appointment> Appointments { get; set; } = default!;

    public DbSet<AppointmentType> AppointmentTypes { get; set; } = default!;

    public DbSet<AppointmentDate> AppointmentDates { get; set; } = default!;

    public DbSet<AppointmentReservedDate> AppointmentReservedDates { get; set; } = default!;

    public DbSet<AppointmentExclusiveHost> AppointmentExclusiveHosts { get; set; } = default!;

    public DbSet<Accomplishment> Accomplishments { get; set; } = default!;

    public DbSet<AccomplishmentScale> AccomplishmentScales { get; set; } = default!;

    public DbSet<AccomplishmentStudent> AccomplishmentStudents { get; set; } = default!;

    public DbSet<AccomplishmentTeacher> AccomplishmentAdditionalTeachers { get; set; } = default!;

    public DbSet<Classroom> Classrooms { get; set; } = default!;

    public DbSet<Classtime> Classtimes { get; set; } = default!;

    public DbSet<ClasstimeShortDay> ClasstimeShortDays { get; set; } = default!;

    public DbSet<Classday> Classdays { get; set; } = default!;

    public DbSet<Timetable> Timetable { get; set; } = default!;

    public DbSet<AccomplishmentAchievement> AccomplishmentAchievements { get; set; } = default!;

    public DbSet<Announcement> Announcements { get; set; } = default!;

    public DbSet<StudentObservation> StudentObservations { get; set; } = default!;
    
    public DbSet<ObservationLesson> ObservationLessons { get; set; } = default!;
    
    public DbSet<ObservationType> ObservationTypes { get; set; } = default!;
    
    public DbSet<ObservationTarget> ObservationTargets { get; set; } = default!;
    
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