namespace API.Services.JobQueue;

using System.Reflection;
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

    public DateTime DequeueAfter { get; set; }

    public bool IsComplete { get; set; }
}

public class JobRecordConfiguration : IEntityTypeConfiguration<JobRecord>
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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

            var typeName = root.GetProperty("$type").GetString()
                           ?? throw new JsonException("Missing $type property.");

            var type = Type.GetType(typeName)
                       ?? throw new JsonException($"Type '{typeName}' not found.");

            return (ICommand) root.Deserialize(type, options)!;
        }

        public override void Write(
            Utf8JsonWriter writer, ICommand value, JsonSerializerOptions options)
        {
            var type = value.GetType();

            writer.WriteStartObject();
            writer.WriteString("$type", type.FullName);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(x => x.CanRead && x.GetCustomAttribute<JsonIgnoreAttribute>() is null))
            {
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name);
                JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType, options);
            }

            writer.WriteEndObject();
        }
    }
}