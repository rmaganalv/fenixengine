namespace Service.IaMaster.Services;


public interface ILocalCopilotService
{
    Task<bool> IsModelDownloadedAsync(string modelAlias);
    Task DownloadModelAsync(string modelAlias, Action<float> onProgress, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> GetCodeCompletionStreamAsync(string prompt, string contextCode = "", CancellationToken cancellationToken = default);
}