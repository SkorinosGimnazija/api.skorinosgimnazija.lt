namespace Application.Posts.Dtos
{
    using System;
    using System.Collections.Generic;
    using Domain.CMS;

    public record PublicPostDto
    {
        public int Id { get; set; } 
        public List<string> Files { get; set; }

        public List<string> Images { get; set; }

        public string IntroText { get; set; }
        
        public string Language { get; set; }

        public DateTime PublishDate { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }
    }
}