namespace Domain.CMS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    public class Post
    {
        public int Id { get; set; }

        public bool IsFeatured { get; set; }

        public List<string> Files { get; set; } 
           
        public List<string> Images { get; set; }

        public string? IntroText { get; set; }

        public bool IsPublished { get; set; }
        public int DomainId { get; set; }
        public int CategoryId { get; set; }

        public Domain Domain { get; set; }
        public Category Category { get; set; }

        public DateTime PublishDate { get; set; }

        public string Slug { get; set; }

        public string? Text { get; set; }

        public string Title { get; set; }
    }
}