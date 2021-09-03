


namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using Application.Categories.Dtos;
    using Domain.CMS;

    public record PostDto
    {
        public int Id { get; init; }

        public bool IsFeatured { get; init; }

        public bool IsPublished { get; init; }

        public CategoryDto Category { get; init; }

        public DateTime PublishDate { get; init; }

        public string Slug { get; init; }

        public string Title { get; init; }
    }
}
