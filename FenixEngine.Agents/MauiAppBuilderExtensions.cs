// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
using FenixEngine.Agents.Services.Ollama;
using Service.Agents.Services.Anthropic;
using FenixEngine.Agents.Services;
using FenixEngine.Agents.Services.OpenAI;
using FenixEngine.Agents.Services.Master;


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
        builder.Services.AddSingleton<IOllamaClient, ServiceOllamaClient>();
        builder.Services.AddTransient<ServiceGenerateOllama>();
        builder.Services.AddTransient<ServiceModelsOllama>();
        builder.Services.AddTransient<ServiceMixOllama>();
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

