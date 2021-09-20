namespace Application.Posts.Dtos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public record PostCreateDto
    {
        [Required]
        public bool IsFeatured { get; init; }

        public IFormFileCollection Files { get; init; }

        public IFormFileCollection Images { get; init; }

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