namespace API.Endpoints.Files.Get;

using API.Services.Storage;

public sealed class GetFilePublicEndpoint(FileManager fileManager)
    : Endpoint<RouteIdRequest<string>>
{
    public override void Configure()
    {
        Get("public/files/{id:maxlength(64)}");
        ResponseCache(31536000); // 1 year
        AllowAnonymous();
    }

    public override async Task HandleAsync(RouteIdRequest<string> req, CancellationToken ct)
    {
        var file = await fileManager.GetFileAsync(req.Id);

        if (file is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.BytesAsync(
            file.Data,
            contentType: file.ContentType,
            cancellation: ct);
    }
}