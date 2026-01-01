namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record UrlOptions
{
    [Required]
    public required string Static { get; [UsedImplicitly] init; }

    [Required]
    public required string Admin { get; [UsedImplicitly] init; }

    [Required]
    public required string Domain { get; [UsedImplicitly] init; }
}