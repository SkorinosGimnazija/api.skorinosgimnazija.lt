namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities;
using Domain.Entities.Appointments;
using Domain.Entities.Bullies;
using Domain.Entities.Identity;
using Domain.Entities.Teacher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public interface IAppDbContext
{
    DbSet<Language> Languages { get; }
    DbSet<Menu> Menus { get; }
    DbSet<Banner> Banners { get;  }
    DbSet<MenuLocation> MenuLocations { get; }
    DbSet<Course> Courses { get; }
        
    DbSet<Post> Posts { get; }

    DbSet<BullyReport> BullyReports { get;  }

    DbSet<ParentAppointment> ParentAppointments { get;}
    DbSet<AppointmentReservedDate> ParentAppointmentReservedDates { get;  }

    DbSet<AppointmentDate> ParentAppointmentDates { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}