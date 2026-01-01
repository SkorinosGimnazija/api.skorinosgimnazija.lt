namespace API.Database.Entities.School;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Timetable : TimetableBase
{
    public Classday Day { get; set; } = null!;

    public int DayId { get; set; }
}

public class TimetableOverride : TimetableBase
{
    public DateOnly Date { get; set; }
}

public class TimetableBase
{
    public Classtime Time { get; set; } = null!;

    public int TimeId { get; set; }

    public Classroom Room { get; set; } = null!;

    public int RoomId { get; set; }

    public string ClassName { get; set; } = null!;
}

public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
{
    public const int ClassNameLength = 128;

    public void Configure(EntityTypeBuilder<Timetable> builder)
    {
        builder.HasKey(x => new { x.DayId, x.TimeId, x.RoomId });

        builder.HasIndex(x => new { x.RoomId, x.DayId });

        builder.Property(x => x.ClassName).HasMaxLength(ClassNameLength);

        builder.HasOne(x => x.Day)
            .WithMany()
            .HasForeignKey(x => x.DayId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Time)
            .WithMany()
            .HasForeignKey(x => x.TimeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TimetableOverrideConfiguration : IEntityTypeConfiguration<TimetableOverride>
{
    public void Configure(EntityTypeBuilder<TimetableOverride> builder)
    {
        builder.HasKey(x => new { x.Date, x.TimeId, x.RoomId });

        builder.HasIndex(x => new { x.Date, x.RoomId });

        builder.Property(x => x.ClassName).HasMaxLength(TimetableConfiguration.ClassNameLength);

        builder.HasOne(x => x.Time)
            .WithMany()
            .HasForeignKey(x => x.TimeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}