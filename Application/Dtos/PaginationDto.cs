namespace Application.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public record PaginationDto
    {
        [Range(1, 20)]
        public int Items { get; init; } = 10;

        [Range(0, int.MaxValue / 20)]
        public int Page { get; init; } = 0;
    }
}