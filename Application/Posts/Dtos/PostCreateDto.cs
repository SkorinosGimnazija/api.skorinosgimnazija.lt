namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;

    public record PostCreateDto
    {
        public bool IsFeatured { get; set; }

        public List<string> Files { get; set; } = new();

        public List<string> Images { get; set; } = new();

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public string? IntroText { get; set; }

        public bool IsPublished { get; set; }

        public int DomainId { get; set; }

        public int CategoryId { get; set; }

        public string Slug { get; set; }

        public string? Text { get; set; }

        public string Title { get; set; }
    }
}