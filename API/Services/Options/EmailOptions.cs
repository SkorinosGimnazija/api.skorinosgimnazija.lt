namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record EmailOptions
{
    [Required]
    public required string SenderName { get; [UsedImplicitly] init; }

    [Required]
    public required string SenderEmail { get; [UsedImplicitly] init; }
}