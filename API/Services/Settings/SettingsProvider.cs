namespace API.Services.Settings;

using System.Text.Json;
using API.Database.Entities.Settings;

public sealed class SettingsProvider(AppDbContext dbContext)
{
    public async Task SaveRandomImageSettings(RandomImageSettings settings)
    {
        var entity =
            await dbContext.Settings.FirstAsync(x => x.Id == SettingsConfiguration.RandomImageId);

        entity.Data = JsonSerializer.Serialize(settings);
        await dbContext.SaveChangesAsync();
    }

    public async Task<RandomImageSettings> GetRandomImageSettings()
    {
        var entity =
            await dbContext.Settings.FirstAsync(x => x.Id == SettingsConfiguration.RandomImageId);

        return JsonSerializer.Deserialize<RandomImageSettings>(entity.Data)!;
    }
}