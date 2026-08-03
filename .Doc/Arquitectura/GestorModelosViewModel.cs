using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class GestorModelosViewModel : ObservableObject
{
    private readonly ModelCatalogService _catalogService;

    [ObservableProperty]
    private ObservableCollection<ModelOption> _modelos = new();

    [ObservableProperty]
    private double _progresoDescarga;

    [ObservableProperty]
    private bool _descargando;

    public GestorModelosViewModel(ModelCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [RelayCommand]
    public async Task CargarCatalogoAsync()
    {
        var lista = await _catalogService.ObtenerModelosDisponiblesAsync();
        Modelos = new ObservableCollection<ModelOption>(lista);
    }

    [RelayCommand]
    public async Task DescargarModeloAsync(ModelOption modeloSeleccionado)
    {
        if (modeloSeleccionado == null || modeloSeleccionado.EstaDescargado) return;

        Descargando = true;
        ProgresoDescarga = 0;

        var progressHandler = new Progress<double>(porcentaje =>
        {
            ProgresoDescarga = porcentaje;
        });

        // Ejecutar descarga
        await _catalogService.DescargarModeloAsync(modeloSeleccionado.Id, progressHandler);

        // Actualizar estado al finalizar
        modeloSeleccionado.EstaDescargado = true;
        Descargando = false;
        
        await Application.Current!.MainPage!.DisplayAlert("Éxito", $"Modelo {modeloSeleccionado.Nombre} descargado y listo para usarse offline.", "OK");
    }
}