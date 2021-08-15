


namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using Domain.CMS;

    public record PostDto
    {
        public int Id { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsPublished { get; set; }

        public Domain Domain { get; set; }
        public Category Category { get; set; }

        public DateTime PublishDate { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }
    }
}
