namespace FenixEngine;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        myFileTree.GoBack();
    }

}
