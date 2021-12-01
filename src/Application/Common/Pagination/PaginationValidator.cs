namespace SkorinosGimnazija.Application.Common.Pagination;

using FluentValidation;

public class PaginationValidator : AbstractValidator<PaginationDto>
{
    public PaginationValidator()
    {
        RuleFor(v => v.Items)
            .InclusiveBetween(1, 20)
            .DependentRules(() =>
            {
                RuleFor(v => v.Page)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(x => int.MaxValue / x.Items);
            });
    }
}