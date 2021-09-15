namespace Application.Posts.Dtos
{
using Application.Categories.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record PostSearchDto
    {
        public string ObjectID { get; init; }
         
        public bool IsPublished { get; init; }

        public DateTime PublishDate { get; init; }

        public string Title { get; init; }
        public string? Text { get; init; }
    }
}
