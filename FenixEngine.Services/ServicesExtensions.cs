using FenixEngine.Service.Src.Prompts;
using FenixEngine.Services.IaAssistant;
using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Services.Src.IaAssistant;
using FenixEngine.Services.Src.Providers;

namespace FenixEngine.Services;

public static class ServicesExtensions
{
    public static MauiAppBuilder UseEngineServices(this MauiAppBuilder builder)
    {

        //builder.Services.AddDbContext<EngineDbContext>();

        // HttpClient Nativo
        builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
        // Servicios de IA Fragmentados
        builder.Services.AddSingleton<IPromptBuilderService, PromptBuilderService>();
        builder.Services.AddTransient<IAIAgentService, AIAgentService>();
        builder.Services.AddSingleton<IApiKeyStore, SecureApiKeyStore>();
        builder.Services.AddSingleton<IAProviderConnector, OpenAiProviderConnector>();
        builder.Services.AddSingleton<IAProviderConnector, GeminiProviderConnector>();
        builder.Services.AddSingleton<IAProviderConnector, OllamaProviderConnector>();
        builder.Services.AddSingleton<IAProviderConnectorFactory, ProviderConnectorFactory>();


        return builder;
    }

}
