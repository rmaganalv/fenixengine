

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenixEngine.DataBase.Src.Services;
using FenixEngine.Services.IaAssistant;
using FenixEngine.Services.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace FenixEngine.Src.ViewModels;

public partial class InitialViewModel : BaseViewModel
{
    private readonly ServiceDbContext _context;
    
    private readonly IAIAgentService _aiAgentService;


    // Configuración del Agente / Servidor
    [ObservableProperty]
    private AgentOptions _options;

    // Campos de Entrada de la UI
    [ObservableProperty]
    private string _systemInstruction = "Eres un asistente técnico experto y conciso.";

    [ObservableProperty]
    private string _userQuery = string.Empty;

    // Campo de Salida de la UI
    [ObservableProperty]
    private string _responseOutput = string.Empty;

    // Colección de Modelos disponibles devueltos por el endpoint /models
    public ObservableCollection<string> AvailableModels { get; } = new();

    // Colección de rutas de archivos Markdown seleccionados (.md)
    public ObservableCollection<string> SelectedMarkdownFiles { get; } = new();

    public InitialViewModel(IAIAgentService aiAgentService, ServiceDbContext context)
    {
        _aiAgentService = aiAgentService;
        _options = new AgentOptions();
        _context = context;
    }

    /// <summary>
    /// Comando para conectar al servidor y listar modelos disponibles.
    /// </summary>
    [RelayCommand]
    private async Task FetchModelsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ResponseOutput = "Conectando al servidor para obtener la lista de modelos...";

            var models = await _aiAgentService.FetchAvailableModelsAsync(Options);

            AvailableModels.Clear();
            foreach (var model in models)
            {
                AvailableModels.Add(model);
            }

            if (AvailableModels.Any())
            {
                Options.SelectedModel = AvailableModels.First();
                OnPropertyChanged(nameof(Options));
            }

            ResponseOutput = $"Modelos cargados correctamente ({AvailableModels.Count} encontrados).";
        }
        catch (Exception ex)
        {
            ResponseOutput = $"Error al obtener modelos: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Comando para abrir el selector de archivos nativo de MAUI y cargar archivos .md
    /// </summary>
    [RelayCommand]
    private async Task PickFilesAsync()
    {
        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.plain-text", "public.text" } },
                    { DevicePlatform.Android, new[] { "text/plain", "text/markdown" } },
                    { DevicePlatform.WinUI, new[] { ".md", ".txt" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.plain-text", "public.text", "md", "txt" } }
                });

            var options = new PickOptions
            {
                PickerTitle = "Selecciona archivos de contexto Markdown",
                FileTypes = customFileType // <-- La propiedad correcta es FileTypes
            };


            var resultList = await FilePicker.PickMultipleAsync( options );
            if (resultList != null)
            {
                SelectedMarkdownFiles.Clear();
                foreach (var file in resultList)
                {
                    SelectedMarkdownFiles.Add(file?.FullPath ?? string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ResponseOutput = $"Error al seleccionar archivos: {ex.Message}";
        }
    }

    /// <summary>
    /// Comando para enviar la consulta con la configuración y archivos adjuntos a la IA.
    /// </summary>
    [RelayCommand]
    private async Task SendRequestAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(UserQuery)) return;

        try
        {
            IsBusy = true;
            ResponseOutput = "Procesando consulta...";

            var response = await _aiAgentService.SendQueryAsync(
                Options,
                SystemInstruction,
                UserQuery,
                SelectedMarkdownFiles
            );

            ResponseOutput = response;
        }
        catch (Exception ex)
        {
            ResponseOutput = $"Error al procesar la solicitud: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Consulta el primer usuario en la base de datos para saludar al iniciar.
    /// </summary>
    public async Task InitializeGreetingAsync()
    {
        try
        {
            var user = await _context.UserArchitects.FirstOrDefaultAsync();
            if (user != null)
            {
                ResponseOutput = $"Hello {user.UserName}?";
            }
            else
            {
                ResponseOutput = "Hello Guest?";
            }
        }
        catch (Exception ex)
        {
            ResponseOutput = $"Error al cargar usuario inicial: {ex.Message}";
        }
    }
}
