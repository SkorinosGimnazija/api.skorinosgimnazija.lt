namespace API.Database;

using System.Reflection;
using API.Database.Entities.Achievements;
using API.Database.Entities.Appointments;
using API.Database.Entities.Authorization;
using API.Database.Entities.BullyReports;
using API.Database.Entities.CMS;
using API.Database.Entities.Courses;
using API.Database.Entities.FailureReports;
using API.Database.Entities.Observations;
using API.Database.Entities.School;
using EntityFramework.Exceptions.PostgreSQL;

public sealed class AppDbContext(DbContextOptions options, IWebHostEnvironment env)
    : DbContext(options)
{
    public DbSet<AppUser> Users { get; init; }

    public DbSet<RefreshToken> RefreshTokens { get; init; }

    public DbSet<Observation> Observations { get; init; }

    public DbSet<ObservationOption> ObservationOptions { get; init; }

    public DbSet<ObservationLesson> ObservationLessons { get; init; }

    public DbSet<ObservationStudent> ObservationStudents { get; init; }

    public DbSet<Appointment> Appointments { get; init; }

    public DbSet<AppointmentType> AppointmentTypes { get; init; }

    public DbSet<AppointmentDate> AppointmentDates { get; init; }

    public DbSet<AppointmentReservedDate> AppointmentReservedDates { get; init; }

    public DbSet<FailureReport> FailureReports { get; init; }

    public DbSet<Achievement> Achievements { get; init; }

    public DbSet<AchievementType> AchievementTypes { get; init; }

    public DbSet<AchievementScale> AchievementScales { get; init; }

    public DbSet<AchievementStudent> AchievementStudents { get; init; }

    public DbSet<Classroom> Classrooms { get; init; }

    public DbSet<Classday> Classdays { get; init; }

    public DbSet<Classtime> Classtimes { get; init; }

    public DbSet<Timetable> Timetable { get; init; }

    public DbSet<TimetableOverride> TimetableOverrides { get; init; }

    public DbSet<ShortDay> ShortDays { get; init; }

    public DbSet<BullyReport> BullyReports { get; init; }

    public DbSet<Course> Courses { get; init; }

    public DbSet<Post> Posts { get; init; }

    public DbSet<Language> Languages { get; init; }

    public DbSet<Banner> Banners { get; init; }

    public DbSet<Menu> Menus { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("pg_trgm");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (env.IsDevelopment())
        {
            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();
            // builder.ConfigureWarnings(c =>
            // {
            //     c.Ignore(RelationalEventId.PendingModelChangesWarning);
            // });
        }

        builder.UseExceptionProcessor();
        base.OnConfiguring(builder);
    }
}