namespace API.Services.Settings;

using System.Text.Json.Serialization;

public record RandomImageSettings
{
    public int CacheDurationInMinutes { get; init; }

    public int? ForcedPostId { get; init; }

    [JsonIgnore]
    public TimeSpan CacheDuration
    {
        get { return TimeSpan.FromMinutes(CacheDurationInMinutes); }
    }
}