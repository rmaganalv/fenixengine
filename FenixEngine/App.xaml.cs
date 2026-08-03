using FenixEngine.Src.Pages;

namespace FenixEngine;

public partial class App : Application
{
	public App(InitialPage initialPage)
	{
		try
		{
			InitializeComponent();
			MainPage = new NavigationPage(initialPage);
		}
		catch (Exception ex)
		{
			// Esto te imprimirá en la consola de VS Code/Visual Studio 
			// exactamente el tipo de servicio que falta registrar.
			System.Diagnostics.Debug.WriteLine($"[DI ERROR] {ex.InnerException?.Message ?? ex.Message}");
			throw;
		}
	}
}
