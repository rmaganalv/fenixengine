using Microsoft.AI.Foundry.Local;

namespace Service.IaMaster.Services;
public class ModelCatalogService
{
    private FoundryLocalManager? _manager;

    private async Task EnsureInitializedAsync()
    {
        if (_manager == null)
        {
            _manager = await FoundryLocalManager.CreateAsync(new Configuration
            {
                AppName = "MauiMasterAgentApp"
            });
        }
    }

    // Obtener la lista de modelos disponibles en el catálogo de Foundry
    public async Task<List<ModelOption>> ObtenerModelosDisponiblesAsync()
    {
        await EnsureInitializedAsync();

        // Obtenemos todos los modelos que ofrece la librería
        var catalogModels = await _manager!.GetCatalogModelsAsync();

        var listaModelos = new List<ModelOption>();

        foreach (var model in catalogModels)
        {
            listaModelos.Add(new ModelOption
            {
                Id = model.Id,                 // Ej: "phi-3.5-mini-instruct"
                Nombre = model.DisplayName,    // Ej: "Phi 3.5 Mini Instruct"
                Descripcion = model.Description,
                EstaDescargado = model.IsDownloaded,
                TamanoEstimadoGB = model.SizeInBytes / (1024.0 * 1024.0 * 1024.0) // Convertir a GB
            });
        }

        return listaModelos;
    }
}

// Clase DTO para vincular fácilmente a la UI de MAUI
public class ModelOption
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EstaDescargado { get; set; }
    public double TamanoEstimadoGB { get; set; }
}