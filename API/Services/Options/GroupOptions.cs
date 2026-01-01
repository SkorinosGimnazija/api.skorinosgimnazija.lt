namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record GroupOptions
{
    [Required]
    public required string Service { get; [UsedImplicitly] init; }

    [Required]
    public required string Managers { get; [UsedImplicitly] init; }

    [Required]
    public required string Teachers { get; [UsedImplicitly] init; }

    [Required]
    public required string SocialManagers { get; [UsedImplicitly] init; }

    [Required]
    public required string TechManagers { get; [UsedImplicitly] init; }
}