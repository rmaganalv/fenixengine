using System;
using System.Globalization;
using Microsoft.Maui.Controls;


namespace FenixEngine.Src.Control;

public class LevelToMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int level = (int)value;
        return new Thickness(level * 20, 0, 0, 0); // 20px por nivel
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
