using FenixEngine.Service.Src.Prompts;
using FenixEngine.Services.IaAssistant;
using FenixEngine.Services.Src.ApiClient;
using FenixEngine.Services.Src.DataAccess;
using FenixEngine.Services.Src.IaAssistant;
using Microsoft.EntityFrameworkCore;

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

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "miapp.db3");
       

        builder.Services.AddDbContext<EngineDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")
        );
        //_dbContext.Database.EnsureCreatedAsync();
        //_dbContext.Dispose();

        return builder;
    }

    // 2. Método de extensión sobre MauiApp para inicializar la DB al arrancar
    public static MauiApp InitializeEngineDatabase(this MauiApp app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<EngineDbContext>();
            // 1. Elimina la base de datos física y todas sus tablas si ya existen
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        return app;
    }

}
