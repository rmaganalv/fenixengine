using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.IA; // Contiene ChatMessage e IChatClient oficiales

namespace SeServices;

public class LocalCopilotService : ILocalCopilotService
{
    private readonly string _defaultModelAlias = "qwen2.5-coder-0.5b";
    private FoundryLocalManager _manager;
    private IChatClient _chatClient;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken = default)
    {
        if (_manager != null) return;

        await _initLock.WaitAsync(cancellationToken);
        try
        {
            if (_manager == null)
            {
                var config = new Configuration { AppName = "Service.IaMaster" };
                await FoundryLocalManager.CreateAsync(config);
                _manager = FoundryLocalManager.Instance;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task<bool> IsModelDownloadedAsync(string modelAlias)
    {
        await EnsureInitializedAsync();
        var catalog = await _manager.GetCatalogAsync();
        var model = await catalog.GetModelAsync(modelAlias);
        return model != null && model.IsDownloaded;
    }

    public async Task DownloadModelAsync(string modelAlias, Action<float> onProgress, CancellationToken cancellationToken = default)
    {
        await EnsureInitializedAsync(cancellationToken);
        var catalog = await _manager.GetCatalogAsync();
        var model = await catalog.GetModelAsync(modelAlias) 
                    ?? throw new InvalidOperationException($"El modelo '{modelAlias}' no se encuentra.");

        await model.DownloadAsync(p => onProgress?.Invoke((float)p), cancellationToken);
    }

    public async IAsyncEnumerable<string> GetCodeCompletionStreamAsync(
        string prompt, 
        string contextCode = "", 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureInitializedAsync(cancellationToken);

        if (_chatClient == null)
        {
            var catalog = await _manager.GetCatalogAsync();
            var model = await catalog.GetModelAsync(_defaultModelAlias);
            await model.LoadAsync(cancellationToken);
            _chatClient = model.CreateChatClient(); // Retorna IChatClient estándar
        }

        var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, "Eres un copiloto de código nativo para macOS."),
        };

        if (!string.IsNullOrWhiteSpace(contextCode))
        {
            messages.Add(new ChatMessage(ChatRole.User, $"Contexto:\n```{contextCode}```"));
        }

        messages.Add(new ChatMessage(ChatRole.User, prompt));

        // Invocación estándar de streaming
        await foreach (var responseChunk in _chatClient.CompleteStreamingAsync(messages, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrEmpty(responseChunk.Text))
            {
                yield return responseChunk.Text;
            }
        }
    }
}