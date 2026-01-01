namespace API.Endpoints.Posts.Update;

using API.Endpoints.Posts.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdatePostRequest : CreatePostRequest
{
    public required int Id { get; init; }

    public List<string>? OldImages { get; init; }

    public string? OldFeaturedImage { get; init; }
}

public class UpdatePostRequestValidator : Validator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        Include(new CreatePostRequestValidator());
    }
}