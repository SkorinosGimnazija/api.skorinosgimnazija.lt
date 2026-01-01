namespace API.Endpoints;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

[PublicAPI]
public record PaginationRequest
{
    [Range(1, 50)]
    [QueryParam]
    public int Items { get; init; } = 20;

    [Range(1, 10_000)]
    [QueryParam]
    public int Page { get; init; } = 1;

    [JsonIgnore]
    public int Skip
    {
        get { return (Page - 1) * Items; }
    }
}