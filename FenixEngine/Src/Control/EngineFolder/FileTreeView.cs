
namespace FenixEngine.Src.Control;
// Control visual


public class FileTreeView : ContentView
{
    public static readonly BindableProperty RootPathProperty =
        BindableProperty.Create(nameof(RootPath), typeof(string), typeof(FileTreeView),
            AppContext.BaseDirectory, propertyChanged: OnRootPathChanged);

    public string RootPath
    {
        get => (string)GetValue(RootPathProperty);
        set => SetValue(RootPathProperty, value);
    }

    private CollectionView _treeView;

    public FileTreeView()
    {
        _treeView = new CollectionView
        {
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid { Padding = 4 };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 30 });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                var icon = new Image { WidthRequest = 20, HeightRequest = 20 };
                icon.SetBinding(Image.SourceProperty, "Icon");

                var label = new Label { VerticalOptions = LayoutOptions.Center };
                label.SetBinding(Label.TextProperty, "Name");

                grid.Add(icon);
                grid.Add(label, 1, 0);

                /*var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    if (grid.BindingContext is FileNode node && node.IsFolder)
                    {
                        node.IsExpanded = !node.IsExpanded;
                        if (node.IsExpanded && node.Children.Count == 0)
                            FileTreeLoader.LoadChildren(node);
                    }
                };
                grid.GestureRecognizers.Add(tapGesture);*/


                var tapGesture = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2 // ahora es doble clic
                };

                tapGesture.Tapped += (s, e) =>
                {
                    if (grid.BindingContext is FileNode node)
                    {
                        if (node.IsFolder)
                        {
                            // Cambiar la raíz a la carpeta seleccionada
                            RootPath = node.Path ?? string.Empty;
                        }
                        else
                        {
                            // Aquí podrías abrir el archivo en tu editor
                            // Por ahora solo mostramos el nombre
                            Application.Current.MainPage.DisplayAlert("Archivo", $"Seleccionaste {node.Name ?? "" }", "OK");
                        }
                    }
                };
                grid.GestureRecognizers.Add(tapGesture);




                return grid;
            })
        };

        Content = _treeView;
        LoadTree(RootPath);
    }

    private static void OnRootPathChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FileTreeView)bindable;
        control.LoadTree((string)newValue);
    }

    private void LoadTree(string path)
    {
        var rootNode = FileTreeLoader.Load(path);
        _treeView.ItemsSource = rootNode.Children;
    }
    
    public void GoBack()
    {
        if (string.IsNullOrEmpty(RootPath)) return;

        var parent = Directory.GetParent(RootPath);
        if (parent != null)
        {
            RootPath = parent.FullName;
        }
    }


}