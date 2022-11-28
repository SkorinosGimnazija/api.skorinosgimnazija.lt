namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppointmentDateConfiguration : IEntityTypeConfiguration<AppointmentDate>
{
    public void Configure(EntityTypeBuilder<AppointmentDate> builder)
    {
        builder.HasOne(x => x.Type).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => new { x.Date, x.TypeId }).IsUnique();
    }
}