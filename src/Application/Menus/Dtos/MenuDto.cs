﻿namespace SkorinosGimnazija.Application.Menus.Dtos;

using Domain.Entities;
using Languages.Dtos;
using Posts.Dtos;

public record MenuDto
{
    public int Id { get; init; }

    public int Order { get; init; }
    public string? Url { get; init; }

    public string Title { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public string Path { get; init; } = default!;

    public bool IsPublished { get; init; }

    public LanguageDto Language { get; init; } = default!;

    public MenuLocationDto MenuLocation { get; init; } = default!;

    public PostDto? LinkedPost { get; set; }

    public int? ParentMenuId { get; init; }

    public List<MenuDto> ChildMenus { get; init; } = new();
}