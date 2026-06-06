namespace FenixEngine.ShellService;

// All the code in this file is included in all platforms.
public static class ServicesExtensions
{
    public static MauiAppBuilder UseShellServices(this MauiAppBuilder builder)
    {
        // Aquí puedes registrar servicios comunes si es necesario
        //builder.Services.AddSingleton<MediaPlayerService>();
        return builder;
    }
    
}
