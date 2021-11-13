﻿namespace Application.Posts.Dtos;

using System.ComponentModel.DataAnnotations;

public record PostSearchDto
{
    [Required]
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public DateTime PublishDate { get; init; }

    [Required]
    public string Title { get; init; } = default!;

    public string? Text { get; init; }
}