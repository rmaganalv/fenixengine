using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FenixEngine.Src.ViewModels;

[INotifyPropertyChanged] // <-- Esto activa el soporte de Source Generators para derivados
public abstract partial class BaseViewModel
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(IsNotBusy)); // Helper útil para IsEnabled en botones
            }
        }
    }

    public bool IsNotBusy => !IsBusy;

    private string? _title;
    public string? Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }
}

