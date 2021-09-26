namespace Application.Categories.Dtos
{
    using System.ComponentModel.DataAnnotations;
    using Languages.Dtos;

    public record CategoryDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        public string Slug { get; init; }

        [Required]
        public LanguageDto Language { get; init; }

        [Required]
        public bool ShowOnHomePage { get; init; }
    }
}