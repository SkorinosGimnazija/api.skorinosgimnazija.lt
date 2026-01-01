namespace API.Database.Entities.Appointments;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentDate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public AppointmentType Type { get; set; } = null!;

    public int TypeId { get; set; }

    public List<AppUser> ReservedHosts { get; set; } = [];
}

public class AppointmentDateConfiguration : IEntityTypeConfiguration<AppointmentDate>
{
    public void Configure(EntityTypeBuilder<AppointmentDate> builder)
    {
        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => new { x.TypeId, x.Date });

        builder.HasMany(x => x.ReservedHosts)
            .WithMany()
            .UsingEntity<AppointmentReservedDate>(
                r => r.HasOne<AppUser>().WithMany().HasForeignKey(x => x.HostId),
                l => l.HasOne<AppointmentDate>().WithMany().HasForeignKey(x => x.DateId));

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}