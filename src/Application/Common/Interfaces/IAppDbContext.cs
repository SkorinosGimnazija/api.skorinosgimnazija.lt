namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities;
using Domain.Entities.Accomplishments;
using Domain.Entities.Appointments;
using Domain.Entities.Bullies;
using Domain.Entities.CMS;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public interface IAppDbContext
{
    DbSet<Language> Languages { get; }

    DbSet<Menu> Menus { get; }

    DbSet<AppUser> Users { get; }

    DbSet<Banner> Banners { get; }

    DbSet<MenuLocation> MenuLocations { get; }

    DbSet<Course> Courses { get; }

    DbSet<Post> Posts { get; }

    DbSet<BullyReport> BullyReports { get; }

    DbSet<Appointment> Appointments { get; }

    DbSet<AppointmentReservedDate> AppointmentReservedDates { get; }

    DbSet<AppointmentDate> AppointmentDates { get; }

    DbSet<AppointmentType> AppointmentTypes { get; }

    DbSet<AppointmentExclusiveHost> AppointmentExclusiveHosts { get; }

    DbSet<Accomplishment> Accomplishments { get; }

    DbSet<AccomplishmentScale> AccomplishmentScales { get;  }

    DbSet<AccomplishmentStudent> AccomplishmentStudents { get; }

    DbSet<AccomplishmentTeacher> AccomplishmentAdditionalTeachers { get;  }

    DbSet<AccomplishmentClassroom> AccomplishmentClassrooms { get; }

    DbSet<AccomplishmentAchievement> AccomplishmentAchievements { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}