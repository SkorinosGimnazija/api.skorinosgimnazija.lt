namespace Application.Languages.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public record LanguageDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        public string Slug { get; init; }
    }
}