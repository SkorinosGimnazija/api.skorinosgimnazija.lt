namespace API.Services.JobQueue;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class JobRecord : IJobStorageRecord
{
    public Guid TrackingID { get; set; }

    public string QueueID { get; set; } = null!;

    public object Command { get; set; } = null!;

    public DateTime ExecuteAfter { get; set; }

    public DateTime ExpireOn { get; set; }

    public bool IsComplete { get; set; }
}

public class JobRecordConfiguration : IEntityTypeConfiguration<JobRecord>
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = { new CommandJsonConverter() }
    };

    public void Configure(EntityTypeBuilder<JobRecord> builder)
    {
        builder.HasKey(x => x.TrackingID);

        builder.Property(x => x.Command)
            .HasConversion(
                x => JsonSerializer.Serialize((ICommand) x, _serializerOptions),
                x => JsonSerializer.Deserialize<ICommand>(x, _serializerOptions)!)
            .HasColumnType("jsonb");
    }

    private class CommandJsonConverter : JsonConverter<ICommand>
    {
        public override ICommand Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            var typeName = root.GetProperty("$type").GetString()!;
            var type = Type.GetType(typeName)!;

            var command = JsonSerializer.Deserialize(root.GetRawText(), type, options)!;
            return (ICommand) command;
        }

        public override void Write(
            Utf8JsonWriter writer, ICommand value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("$type", value.GetType().FullName);

            var properties = value.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead)
                {
                    writer.WritePropertyName(property.Name);
                    JsonSerializer.Serialize(writer, property.GetValue(value), options);
                }
            }

            writer.WriteEndObject();
        }
    }
}