
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration;
using FenixEngine.Desktop.ToolBar;
using FenixEngine.Desktop.Platforms.MacCatalyst;
using FenixEngine.Src.Pages;




#if MACCATALYST
using UIKit;
using Foundation;
#endif

namespace FenixEngine;

public partial class App : Application
{
	//private readonly DesktopToolbar _toolbar;
	public App()
	{
		InitializeComponent();
		//_toolbar = toolbar;
	}

/*
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage());

        window.Created += OnWindowCreated;

        return window;
    }

    private void OnWindowCreated(object? sender, EventArgs e)
    {
#if MACCATALYST
        if (sender is Window window)
        {
            (_toolbar as MacDesktopToolbar)?.ShowTestToolbar();
        }
#endif
    }
*/
	protected override Window CreateWindow(IActivationState? activationState)
	{
    	var window = new Window(new InitialPage());

		#if MACCATALYST
				window.TitleBar = CreateTitleBar();
		#endif

		return window;
	}


	private TitleBar CreateTitleBar()
	{
		var titleBar = new TitleBar();

		// Título
		titleBar.Title = "Fenix Engine";

		// Contenido personalizado
		titleBar.Content = CreateToolbarContent();

		return titleBar;
	}

	private View CreateToolbarContent()
	{
		var layout = new HorizontalStackLayout
		{
			Spacing = 10,
			Padding = new Thickness(10,0),
			VerticalOptions = LayoutOptions.Center
		};

		var btnBack = new Button
		{
			Text = "←",
			WidthRequest = 40
		};

		var search = new Entry
		{
			Placeholder = "Buscar...",
			WidthRequest = 250
		};

		var btnSearch = new Button
		{
			Text = "Buscar"
		};

		layout.Add(btnBack);
		layout.Add(search);
		layout.Add(btnSearch);

		return layout;
	}


}
