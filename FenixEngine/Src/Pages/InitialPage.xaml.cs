using FenixEngine.Src.ViewModels;

namespace FenixEngine.Src.Pages;

public partial class InitialPage : ContentPage
{
    private readonly InitialViewModel _viewModel;
    
    public InitialPage(InitialViewModel initialPage)
    {
        InitializeComponent();
        BindingContext = _viewModel = initialPage;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 🚀 Carga el saludo inicial directamente en ResponseOutput al mostrar la vista
        if (_viewModel != null)
        {
            await _viewModel.InitializeGreetingAsync();
        }
    }

}