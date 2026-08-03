using System.Collections.ObjectModel;
using FenixEngine.Src.Control;

namespace FenixEngine.Src.Pages;

public partial class MainPage : ContentPage
{
    private SolutionExplorerViewModel _vm;
    public MainPage()
    {
        InitializeComponent();
        _vm = new SolutionExplorerViewModel();
        BindingContext = _vm;
        try
        {
            _vm.LoadSolution("/Users/rmagana/Documents/Local/Proyects/MediaPlayer");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar solución: {ex.Message}");
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is SolutionItem item)
        {
            _vm.OnItemTapped(item);
        }
    }
}
