namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record CloudinaryOptions
{
    [Required]
    public required string Url { get; [UsedImplicitly] init; }
}