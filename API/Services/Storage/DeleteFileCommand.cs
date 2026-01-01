namespace API.Services.Storage;

using JetBrains.Annotations;

public class DeleteFileCommand : ICommand
{
    public required IEnumerable<string> FileIds { get; init; }
}

[UsedImplicitly]
public sealed class DeleteFileCommandHandler(FileManager fileManager)
    : ICommandHandler<DeleteFileCommand>
{
    public async Task ExecuteAsync(DeleteFileCommand command, CancellationToken ct)
    {
        foreach (var fileId in command.FileIds)
        {
            await fileManager.DeleteFileAsync(fileId);
        }
    }
}