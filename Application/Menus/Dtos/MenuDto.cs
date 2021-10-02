namespace Application.Menus.Dtos
{
    using System.ComponentModel.DataAnnotations;
    using Languages.Dtos;

    public record MenuDto
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
        public LanguageDto Language { get; init; }

        public string? Url { get; init; }

        public int? ParentMenuId { get; init; }
    }
}