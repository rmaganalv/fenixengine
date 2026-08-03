using FenixEngine.Agents;
using FenixEngine.Shared;

namespace FenixEngine.AppCore;

public static class AppCoreExtension
{
    public static MauiAppBuilder UseEngineAppCore(this MauiAppBuilder builder)
    {
		builder.UseAgentsServices();
		builder.UseSharedServices();
        return builder;        
    }
}