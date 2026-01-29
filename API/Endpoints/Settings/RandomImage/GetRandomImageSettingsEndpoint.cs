namespace API.Endpoints.Settings.RandomImage;

using API.Services.Settings;

public sealed class GetRandomImageSettingsEndpoint(SettingsProvider settings)
    : EndpointWithoutRequest<RandomImageSettings>
{
    public override void Configure()
    {
        Get("settings/random-image");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var randomImageSettings = await settings.GetRandomImageSettings();
        await Send.OkAsync(randomImageSettings, ct);
    }
}