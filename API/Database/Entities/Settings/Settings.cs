namespace API.Database.Entities.Settings;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Settings
{
    public string Id { get; set; } = null!;

    public string Data { get; set; } = null!;
}

public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public const string RandomImageId = "RandomImageSettings";

    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Data).HasColumnType("jsonb");
    }
}