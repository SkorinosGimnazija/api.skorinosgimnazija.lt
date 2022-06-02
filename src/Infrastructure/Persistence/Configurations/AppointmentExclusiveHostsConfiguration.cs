namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppointmentExclusiveHostsConfiguration : IEntityTypeConfiguration<AppointmentExclusiveHost>
{
    public void Configure(EntityTypeBuilder<AppointmentExclusiveHost> builder)
    {
        builder.HasOne(x => x.Type).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserName).HasMaxLength(100);

        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => new { x.UserName, x.TypeId }).IsUnique();
    }
}