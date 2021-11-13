﻿namespace Application.Categories.Dtos;

using System.ComponentModel.DataAnnotations;

public record CategoryCreateDto
{
    [Required]
    public int LanguageId { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    [Required]
    public string Slug { get; init; } = default!;

    [Required]
    public bool ShowOnHomePage { get; init; }
}