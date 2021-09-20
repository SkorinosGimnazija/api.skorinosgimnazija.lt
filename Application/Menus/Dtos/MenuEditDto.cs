namespace Application.Menus.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public record MenuEditDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public int Order { get; init; }

        [Required]
        public string Name { get; init; }

        public string? Slug { get; init; }

        [Required]
        public bool IsPublished { get; init; }

        [Required]
        public int CategoryId { get; init; }

        public string? Url { get; init; }

        public int? ParentMenuId { get; init; }
    }
}