namespace API.Services.ImageOptimization;

using JetBrains.Annotations;

public class DeleteOptimizedImageCommand : ICommand
{
    public required string ImageTag { get; init; }
}

[UsedImplicitly]
public sealed class DeleteOptimizedImageCommandHandler(IImageOptimizer imageOptimizer)
    : ICommandHandler<DeleteOptimizedImageCommand>
{
    public async Task ExecuteAsync(DeleteOptimizedImageCommand command, CancellationToken ct)
    {
        await imageOptimizer.DeleteFilesByTagAsync(command.ImageTag);
    }
}