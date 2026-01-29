namespace API.Endpoints.Settings.RandomImage;

using API.Services.Settings;

public sealed class PostRandomImageSettingsEndpoint(SettingsProvider settings)
    : Endpoint<RandomImageSettings, EmptyResponse>
{
    public override void Configure()
    {
        Post("settings/random-image");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RandomImageSettings req, CancellationToken ct)
    {
        await settings.SaveRandomImageSettings(req);
        await Send.OkAsync(new(), ct);
    }
}