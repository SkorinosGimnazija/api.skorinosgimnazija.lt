namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record PostRevalidationOptions
{
    [Required]
    public required string Token { get; [UsedImplicitly] init; }

    [Required]
    public required string Url { get; [UsedImplicitly] init; }
}