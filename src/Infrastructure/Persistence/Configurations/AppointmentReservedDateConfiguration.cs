namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppointmentReservedDateConfiguration : IEntityTypeConfiguration<AppointmentReservedDate>
{
    public void Configure(EntityTypeBuilder<AppointmentReservedDate> builder)
    {
        builder.HasOne(x => x.Date).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserName).HasMaxLength(100);

        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => new { x.UserName, x.DateId }).IsUnique();
    }
}