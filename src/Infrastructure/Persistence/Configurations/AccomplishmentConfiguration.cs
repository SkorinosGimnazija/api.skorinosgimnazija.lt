namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkorinosGimnazija.Domain.Entities.Accomplishments;

internal class AccomplishmentConfiguration : IEntityTypeConfiguration<Accomplishment>
{
    public void Configure(EntityTypeBuilder<Accomplishment> builder)
    {
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Scale).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AdditionalTeachers).WithOne(x=> x.Accomplishment).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Students).WithOne(x => x.Accomplishment).OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name).HasMaxLength(256);

        builder.HasIndex(x => x.Date);
    }
}