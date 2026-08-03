using FenixEngine.Agents.Services.Ollama;
using Service.Agents.Services.Anthropic;
using FenixEngine.Agents.Services;
using FenixEngine.Agents.Services.OpenAI;

//using Microsoft.Extensions.DependencyInjection;

namespace FenixEngine.Agents;

public static class ServiceCollectionExtensions
{

    public static MauiAppBuilder UseAgentsServices(this MauiAppBuilder builder)
    {
        builder.UseOllamaServices();
        builder.UseAnthropicServices();
        builder.UseGeminiServices();
        builder.UseOpenAIServices();

        return builder;
    }

    public static MauiAppBuilder UseOllamaServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAnthropicClient, ServiceAnthropicClient>();
        builder.Services.AddTransient<ServiceEmbeddingsAnthropic>();
        builder.Services.AddTransient<ServiceMessagesAnthropic>();
        builder.Services.AddTransient<ServiceModelsAnthorpic>();
        return builder;
    }

    public static MauiAppBuilder UseAnthropicServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAnthropicClient, ServiceAnthropicClient>();
        builder.Services.AddTransient<ServiceEmbeddingsAnthropic>();
        builder.Services.AddTransient<ServiceMessagesAnthropic>();
        builder.Services.AddTransient<ServiceModelsAnthorpic>();
        return builder;
    }

    public static MauiAppBuilder UseGeminiServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAnthropicClient, ServiceAnthropicClient>();
        builder.Services.AddTransient<ServiceEmbeddingsAnthropic>();
        builder.Services.AddTransient<ServiceMessagesAnthropic>();
        builder.Services.AddTransient<ServiceModelsAnthorpic>();
        return builder;
    }


    public static MauiAppBuilder UseOpenAIServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IOpenAIClient, ServiceOpenAIClient>();
        builder.Services.AddTransient<ServiceEmbeddingOpenAI>();
        builder.Services.AddTransient<ServiceModelsOpenAI>();
        builder.Services.AddTransient<ServiceChatOpenAI>();
        return builder;
    }

}

