namespace API.Endpoints.Menus.Create;

using API.Database.Entities.CMS;
using JetBrains.Annotations;

[PublicAPI]
public record CreateMenuRequest
{
    public required int Order { get; init; }

    public required bool IsPublished { get; init; }

    public required bool IsHidden { get; init; }

    public required string Title { get; init; }

    public required string LanguageId { get; init; }

    public string? Url { get; init; }

    public int? PostId { get; init; }

    public int? ParentMenuId { get; init; }
}

public class CreateMenuRequestValidator : Validator<CreateMenuRequest>
{
    public CreateMenuRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(MenuConfiguration.TitleLength);

        RuleFor(x => x.Url)
            .MaximumLength(MenuConfiguration.UrlLength);

        RuleFor(x => x.Url)
            .NotEmpty()
            .When(x => x.PostId is not null);

        RuleFor(x => x.ParentMenuId)
            .MustAsync(ValidParentMenu)
            .WithMessage("Menu with submenus cannot have a URL");
    }

    private async Task<bool> ValidParentMenu(int? parentMenuId, CancellationToken ct)
    {
        if (parentMenuId is null)
        {
            return true;
        }

        var dbContext = Resolve<AppDbContext>();
        var parentValid = await dbContext.Menus
                              .AnyAsync(x => x.Id == parentMenuId &&
                                             x.Url == null, ct);

        if (!parentValid)
        {
            return false;
        }

        return true;
    }
}