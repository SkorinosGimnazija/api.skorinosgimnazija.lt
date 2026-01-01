namespace API.Endpoints.Menus.Update;

using API.Endpoints.Menus.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateMenuRequest : CreateMenuRequest
{
    public required int Id { get; init; }
}

public class UpdateMenuRequestValidator : Validator<UpdateMenuRequest>
{
    public UpdateMenuRequestValidator()
    {
        Include(new CreateMenuRequestValidator());
        RuleFor(x => x.ParentMenuId).NotEqual(x => x.Id);
    }
}