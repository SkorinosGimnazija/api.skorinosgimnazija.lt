namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;

    public record PostCreateDto
    {
        public bool IsFeatured { get; init; }

        public List<IFormFile> Files { get; init; } = new();

        public List<IFormFile> Images { get; init; } = new();

        public DateTime PublishDate { get; init; } = DateTime.Now;

        public string? IntroText { get; init; }

        public bool IsPublished { get; init; }


        public int CategoryId { get; init; }

        public string Slug { get; init; }

        public string? Text { get; init; }

        public string Title { get; init; }
    }
}