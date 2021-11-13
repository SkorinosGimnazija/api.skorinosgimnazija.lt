﻿namespace Domain.CMS;

public class Post
{
    public int Id { get; set; }

    public bool IsFeatured { get; set; }

    public List<string>? Files { get; set; }

    public List<string>? Images { get; set; }

    public string? IntroText { get; set; }

    public bool IsPublished { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public DateTime PublishDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string Slug { get; set; } = default!;

    public string? Text { get; set; }

    public string Title { get; set; } = default!;
}