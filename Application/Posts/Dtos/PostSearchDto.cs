namespace Application.Posts.Dtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public record PostSearchDto
    {
        [Required]
        // ReSharper disable once InconsistentNaming
        public string ObjectID { get; init; }

        [Required]
        public bool IsPublished { get; init; }

        [Required]
        public DateTime PublishDate { get; init; }

        [Required]
        public string Title { get; init; }

        public string? Text { get; init; }
    }
}