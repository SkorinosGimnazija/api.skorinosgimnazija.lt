namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Categories.Dtos;

    public record PostDetailsDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public int CategoryId { get; init; }

        [Required]
        public bool IsFeatured { get; init; }

        [Required]
        public List<string> Files { get; init; }

        [Required]
        public List<string> Images { get; init; }

        public string? IntroText { get; init; }

        [Required]
        public bool IsPublished { get; init; }

        [Required]
        public CategoryDto Category { get; init; }

        [Required]
        public DateTime PublishDate { get; init; }

        public DateTime? ModifiedDate { get; init; }

        [Required]
        public string Slug { get; init; }

        public string? Text { get; init; }

        [Required]
        public string Title { get; init; }
    }
}