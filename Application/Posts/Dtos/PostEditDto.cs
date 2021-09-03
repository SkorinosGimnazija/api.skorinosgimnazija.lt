using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Dtos
{
   public record PostEditDto
    {
        public int Id { get; init; }
        public bool IsFeatured { get; init; }

        public List<string> Files { get; init; } = new();

        public List<string> Images { get; init; } = new();

        public DateTime PublishDate { get; init; } = DateTime.Now;

        public string? IntroText { get; init; }

        public bool IsPublished { get; init; }


        public int CategoryId { get; init; }

        public string Slug { get; init; }

        public string? Text { get; init; }

        public string Title { get; init; }
    } 
}
