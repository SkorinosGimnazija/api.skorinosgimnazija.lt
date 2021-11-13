namespace API.Filters;

using System.Text.Json.Serialization;

public class ExceptionMessage
{
    [JsonPropertyName("@type")]
    public string Type { get; } = "MessageCard";

    [JsonPropertyName("@context")]
    public string Context { get; } = "http://schema.org/extensions";

    public string ThemeColor { get; set; } = "FF0000";

    public string Title { get; set; } = default!;

    public string Text { get; set; } = default!;
}