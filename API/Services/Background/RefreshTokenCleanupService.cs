namespace API.Services.Background;

public class RefreshTokenCleanupService(
    IServiceProvider services,
    ILogger<RefreshTokenCleanupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.UtcNow;
            var count = await dbContext.RefreshTokens
                            .Where(x => x.ExpiresAt < now)
                            .ExecuteDeleteAsync(ct);

            logger.LogInformation("Deleted {Count} expired refresh tokens", count);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception e)
        {
            logger.LogError(e, "Token cleanup failed");
        }
    }
}