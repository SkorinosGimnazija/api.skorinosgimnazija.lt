namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record CaptchaOptions
{
    [Required]
    public required string Secret { get; [UsedImplicitly] init; }
}