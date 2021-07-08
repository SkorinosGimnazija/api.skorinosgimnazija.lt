namespace Persistence
{
    using System;

    public record Post
    {
        public string Body { get; init; }

        public Guid Id { get; init; }

        public DateTime PublishDate { get; init; }

        public string Title { get; init; }
    }
}