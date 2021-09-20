namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public record PostEditDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public bool IsFeatured { get; init; }

        public List<string> Files { get; init; } = new();

        public List<string> Images { get; init; } = new();

        public DateTime PublishDate { get; init; } = DateTime.Now;

        public string? IntroText { get; init; }

        [Required]
        public bool IsPublished { get; init; }

        [Required]
        public int CategoryId { get; init; }

        [Required]
        public string Slug { get; init; }

        public string? Text { get; init; }

        [Required]
        public string Title { get; init; }
    }
}