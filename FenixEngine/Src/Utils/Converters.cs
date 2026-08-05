using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace FenixEngine.Src.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color UserColor { get; set; } = Colors.DodgerBlue;
        public Color BotColor { get; set; } = Colors.Gray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUser)
                return isUser ? UserColor : BotColor;

            return BotColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
