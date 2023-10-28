namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ClasstimeShortDaysConfiguration : IEntityTypeConfiguration<ClasstimeShortDay>
{
    public void Configure(EntityTypeBuilder<ClasstimeShortDay> builder)
    {
        builder.HasIndex(x => x.Date).IsUnique();
    }
}
