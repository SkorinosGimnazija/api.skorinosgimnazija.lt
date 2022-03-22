namespace SkorinosGimnazija.Application.Common.Pagination;

public record PaginationDto
{
    public int Items { get; init; } = 10;

    public int Page { get; init; } = 0;
}