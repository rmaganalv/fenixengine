using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace SeServices;

// 1. Herramienta (Plugin) que el Agente Maestro usa para invocar Agentes Cloud/APIs
public class RemoteAgentPlugin
{
    private readonly IChatClient _cloudAgentClient;

    public RemoteAgentPlugin(IChatClient cloudAgentClient)
    {
        _cloudAgentClient = cloudAgentClient;
    }

    [KernelFunction, Description("Delega tareas complejas, análisis pesados o búsqueda a un agente avanzado en la nube.")]
    public async Task<string> EjecutarAgenteCloudAsync(string instruccionEspecializada)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "Eres un agente especializado en la nube para procesamiento avanzado."),
            new(ChatRole.User, instruccionEspecializada)
        };

        var response = await _cloudAgentClient.CompleteAsync(messages);
        return response.Message.Text ?? "Sin respuesta del agente remoto.";
    }
}

// 2. Orquestador Principal (El Agente Maestro)
public class MasterOrchestratorService
{
    private IChatClient? _masterLocalClient;
    private IChatClient _cloudClient;
    private FoundryLocalManager? _foundryManager;

    public MasterOrchestratorService()
    {
        // Inicializar cliente cloud (ej. Azure/OpenAI o un endpoint API REST)
        _cloudClient = new AzureOpenAIChatClient(/* Configuración API */);
    }

    // Inicializa el modelo maestro elegido o descargado por el usuario
    public async Task CargarModeloMaestroLocalAsync(string nombreModelo = "phi-3.5-mini")
    {
        _foundryManager = await FoundryLocalManager.CreateAsync(new Configuration { AppName = "MauiMasterAgent" });
        
        var model = await _foundryManager.GetModelAsync(nombreModelo);
        
        // Si no está descargado, se descarga a través de Foundry Local
        if (!model.IsDownloaded)
        {
            await model.DownloadAsync();
        }

        await model.LoadAsync();
        
        // Adaptamos el cliente local a la interfaz unificada IChatClient
        _masterLocalClient = model.GetChatClient().AsChatClient();
    }

    // Procesa la solicitud del usuario mediante el Agente Maestro
    public async Task<string> ProcesarSolicitudAsync(string peticionUsuario)
    {
        if (_masterLocalClient == null)
            throw new InvalidOperationException("El agente maestro local no ha sido cargado.");

        // Definimos las herramientas/plugins que el Maestro tiene a su disposición
        var options = new ChatOptions
        {
            Tools = new List<AIFunction>
            {
                // El Maestro tiene la capacidad de llamar al agente en la nube como una función
                AIFunctionFactory.Create(new RemoteAgentPlugin(_cloudClient).EjecutarAgenteCloudAsync)
            }
        };

        var systemPrompt = @"Eres el Agente Maestro Local. Tu trabajo es:
        1. Resolver peticiones del usuario localmente siempre que sea posible.
        2. Si la tarea requiere capacidades superiores o acceso a APIs externas, usa la función 'EjecutarAgenteCloudAsync' para delegar la tarea al Agente Remoto.
        3. Responder al usuario de manera clara y supervisada.";

        var history = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, peticionUsuario)
        };

        // El modelo local analiza la intención y decide si responde solo o si llama a la API Cloud
        var result = await _masterLocalClient.CompleteAsync(history, options);
        return result.Message.Text ?? "Procesado correctamente.";
    }
}