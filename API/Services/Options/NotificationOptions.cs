namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record NotificationOptions
{
    [Required]
    public required string SocialEmail { get; [UsedImplicitly] init; }

    [Required]
    public required string TechEmail { get; [UsedImplicitly] init; }
}