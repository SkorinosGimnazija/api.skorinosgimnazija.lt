namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record GoogleDriveOptions
{
    [Required]
    public required string FolderId { get; [UsedImplicitly] init; }

    [Required]
    public required string User { get; [UsedImplicitly] init; }
}