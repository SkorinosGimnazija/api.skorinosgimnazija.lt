namespace API.Services.JobQueue;

public sealed class JobStorageProvider(IDbContextFactory<AppDbContext> dbContextFactory)
    : IJobStorageProvider<JobRecord>
{
    public async Task StoreJobAsync(JobRecord job, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        dbContext.Set<JobRecord>().Add(job);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(
        PendingJobSearchParams<JobRecord> jobParams)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext.Set<JobRecord>()
                   .Where(jobParams.Match)
                   .OrderBy(x => x.ExecuteAfter)
                   .Take(jobParams.Limit)
                   .ToListAsync();
    }

    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        await dbContext
            .Set<JobRecord>()
            .Where(x => x.TrackingID == job.TrackingID)
            // .ExecuteUpdateAsync(x => x.SetProperty(j => j.IsComplete, true), ct);
            .ExecuteDeleteAsync(ct);
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        await dbContext
            .Set<JobRecord>()
            .Where(x => x.TrackingID == trackingId)
            // .ExecuteUpdateAsync(x => x.SetProperty(j => j.IsComplete, true), ct);
            .ExecuteDeleteAsync(ct);
    }

    public async Task OnHandlerExecutionFailureAsync(
        JobRecord job, Exception exception, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        await dbContext
            .Set<JobRecord>()
            .Where(x => x.TrackingID == job.TrackingID)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(j => j.ExecuteAfter, DateTime.UtcNow.AddMinutes(3)), ct);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> jobParams)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Set<JobRecord>().Where(jobParams.Match).ExecuteDeleteAsync();
    }
}