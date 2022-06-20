namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.Accomplishments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AccomplishmentScaleConfiguration : IEntityTypeConfiguration<AccomplishmentScale>
{
    public void Configure(EntityTypeBuilder<AccomplishmentScale> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
    }
}