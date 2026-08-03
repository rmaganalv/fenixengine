using FenixEngine.Shared.Services;

namespace FenixEngine.Shared;

public static class SharedExtension
{
    public static MauiAppBuilder UseSharedServices(this MauiAppBuilder builder)
    {

        builder.Services.AddSingleton<IFileService,ServiceFile>();
        builder.Services.AddSingleton<IFolderService,ServiceFolder>();
        builder.Services.AddSingleton<ITerminalService,ServiceTerminal>();

        return builder;
    }
}