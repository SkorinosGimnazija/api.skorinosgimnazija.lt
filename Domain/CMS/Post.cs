namespace Domain.CMS
{
    using System;

    public record Post
    {
        public string Body { get; set; }

        public int Id { get; init; }

        public DateTime PublishDate { get; set; }

        public string Title { get; set; }
    }
}