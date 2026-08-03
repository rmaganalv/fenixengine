using FenixEngine.Src.Control;
using System;
using Microsoft.Maui.Controls;
using FenixEngine.Src.Control.Speaches;
using FenixEngine.Src.ViewModels;

namespace FenixEngine.Src.Pages;

public partial class InitialPage : ContentPage
{
    //private readonly NativeTtsService _ttsService;
    //private readonly ISpeechToTextService _speechService;
    private readonly InitialViewModel _viewModel;
    
    public InitialPage(InitialViewModel initialPage)
    {
        InitializeComponent();
        BindingContext = _viewModel = initialPage;
        //_ttsService = new NativeTtsService();
        //_speechService = new SpeechToTextService();
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


/*
    private async void OnSpeakClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TextInput.Text))
            return;

        await _ttsService.SpeakAsync(TextInput.Text);
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        _ttsService.CancelSpeech();
    }

    private async void OnStartRecognitionClicked(object sender, EventArgs e)
    {
        try
        {
            var text = await _speechService.RecognizeAsync();
            ResultEditor.Text = text;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }*/

}