namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppointmentDateConfiguration : IEntityTypeConfiguration<AppointmentDate>
{
    public void Configure(EntityTypeBuilder<AppointmentDate> builder)
    {
        builder.HasOne(x => x.Type).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Date);

        builder.HasIndex(x => x.Date);
    }
}