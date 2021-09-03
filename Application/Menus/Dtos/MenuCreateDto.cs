﻿namespace Domain.CMS
{
    public record MenuCreateDto
    {
        public int Order { get; init; }
        public string Name { get; init; }
        public bool IsPublished { get; init; }
        public string? Slug { get; init; }
        public int CategoryId { get; init; }
        public string? Url { get; init; }
        public int? ParentMenuId { get; init; }
    }
}