namespace API.Database.Entities.CMS;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Language
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;
}

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Id).HasMaxLength(5);

        builder.Property(x => x.Name).HasMaxLength(64);
    }
}